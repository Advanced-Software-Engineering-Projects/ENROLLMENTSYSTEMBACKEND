using Microsoft.AspNetCore.Identity;
using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.IRepositories;
using ENROLLMENTSYSTEMBACKEND.IServices;
using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IPrerequisiteRepository _prerequisiteRepository;
        private readonly IFeeRepository _feeRepository;
        private readonly ISystemConfigRepository _systemConfigRepository;
        private readonly IProgramVersionRepository _programVersionRepository;
        private readonly IProgramCoursesRepository _programCoursesRepository;
        private readonly IPasswordHasher<Student> _passwordHasher;

        public StudentService(
            IStudentRepository studentRepository,
            IEnrollmentRepository enrollmentRepository,
            ICourseRepository courseRepository,
            IPrerequisiteRepository prerequisiteRepository,
            IFeeRepository feeRepository,
            ISystemConfigRepository systemConfigRepository,
            IProgramVersionRepository programVersionRepository,
            IProgramCoursesRepository programCoursesRepository,
            IPasswordHasher<Student> passwordHasher)
        {
            _studentRepository = studentRepository;
            _enrollmentRepository = enrollmentRepository;
            _courseRepository = courseRepository;
            _prerequisiteRepository = prerequisiteRepository;
            _feeRepository = feeRepository;
            _systemConfigRepository = systemConfigRepository;
            _programVersionRepository = programVersionRepository;
            _programCoursesRepository = programCoursesRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<StudentDto> AuthenticateStudentAsync(string studentId, string password)
        {
            var student = await _studentRepository.GetStudentByIdAsync(studentId);
            if (student == null || !BCrypt.Net.BCrypt.Verify(password, student.Password))
                return null;

            return new StudentDto
            {
                StudentId = student.StudentId,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Email = student.Email,
                Phone = student.Phone,
                ProgramVersionId = student.ProgramVersionId
            };
        }

        public async Task<StudentDto> LoginAsync(string studentId, string password)
        {
            var student = await _studentRepository.GetStudentByIdAsync(studentId);
            if (student == null || _passwordHasher.VerifyHashedPassword(student, student.Password, password) != PasswordVerificationResult.Success)
                throw new UnauthorizedAccessException("Invalid credentials");

            return MapToStudentDto(student);
        }

        public async Task<StudentDto> RegisterAsync(StudentDto studentDto)
        {
            var student = new Student
            {
                StudentId = studentDto.StudentId,
                LastName = studentDto.LastName,
                FirstName = studentDto.FirstName,
                DOB = studentDto.DOB,
                Email = studentDto.Email,
                Phone = studentDto.Phone,
                ProgramVersionId = studentDto.ProgramVersionId,
                EnrollmentYear = studentDto.EnrollmentYear,
                Password = _passwordHasher.HashPassword(null, "defaultPassword") // Set default, update later
            };
            await _studentRepository.AddStudentAsync(student);
            return MapToStudentDto(student);
        }

        public async Task<StudentDto> UpdateProfileAsync(string studentId, StudentDto studentDto)
        {
            var student = await _studentRepository.GetStudentByIdAsync(studentId);
            if (student == null) throw new KeyNotFoundException("Student not found");

            student.LastName = studentDto.LastName;
            student.FirstName = studentDto.FirstName;
            student.DOB = studentDto.DOB;
            student.Email = studentDto.Email;
            student.Phone = studentDto.Phone;
            await _studentRepository.UpdateStudentAsync(student);
            return MapToStudentDto(student);
        }

        public async Task<IEnumerable<CourseDto>> GetAvailableCoursesAsync(string studentId, string semester)
        {
            var enrolledCourseIds = (await _enrollmentRepository.GetEnrollmentsByStudentAndSemesterAsync(studentId, semester))
                .Select(e => e.CourseId)
                .ToHashSet();

            var completedCourseIds = (await _enrollmentRepository.GetEnrollmentsByStudentAndStatusAsync(studentId, "Completed"))
                .Select(e => e.CourseId)
                .ToHashSet();

            var allCourses = (await _courseRepository.GetAllCoursesAsync())
                .Where(c => c.SemesterOffered == semester && !enrolledCourseIds.Contains(c.CourseId))
                .ToList();

            var prereqMap = new Dictionary<int, List<int>>();
            foreach (var course in allCourses)
            {
                var prereqs = await _prerequisiteRepository.GetPrerequisitesForCourseAsync(course.CourseId);
                prereqMap[course.CourseId] = prereqs.Select(p => p.PrerequisiteCourseId).ToList();
            }

            return allCourses
                .Where(c => !prereqMap.ContainsKey(c.CourseId) || prereqMap[c.CourseId].All(prereqId => completedCourseIds.Contains(prereqId)))
                .Select(MapToCourseDto);
        }

        public async Task<IEnumerable<CourseDto>> SearchCoursesAsync(string studentId, string? keyword, string? semester, int? creditsMin, int? creditsMax)
        {
            var courses = await _courseRepository.GetAllCoursesAsync();
            return courses.Where(c =>
                (string.IsNullOrEmpty(keyword) || c.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase) || c.Code.Contains(keyword, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(semester) || c.SemesterOffered == semester) &&
                (!creditsMin.HasValue || c.Credits >= creditsMin.Value) &&
                (!creditsMax.HasValue || c.Credits <= creditsMax.Value)).Select(MapToCourseDto);
        }

        public async Task<bool> RegisterCourseAsync(string studentId, int courseId, string semester)
        {
            if ((await _systemConfigRepository.GetConfigValueAsync("Registration:IsOpen")) != "true") throw new InvalidOperationException("Registration is closed");
            if (await HasFeeHoldsAsync(studentId)) throw new InvalidOperationException("Fee holds prevent registration");

            var prerequisites = await _prerequisiteRepository.GetPrerequisitesForCourseAsync(courseId);
            var completedCourseIds = (await _enrollmentRepository.GetEnrollmentsByStudentAndStatusAsync(studentId, "Completed")).Select(e => e.CourseId);
            if (prerequisites.Any(p => !completedCourseIds.Contains(p.PrerequisiteCourseId))) throw new InvalidOperationException("Prerequisites not met");

            var currentEnrollments = await _enrollmentRepository.GetEnrollmentsByStudentAndSemesterAsync(studentId, semester);
            var course = await _courseRepository.GetCourseByIdAsync(courseId);
            var totalCredits = currentEnrollments.Sum(e => _courseRepository.GetCourseByIdAsync(e.CourseId).Result.Credits) + course.Credits;
            if (totalCredits > 20) throw new InvalidOperationException("Exceeds credit limit");

            var enrollment = new Enrollment { StudentId = studentId, CourseId = courseId, Semester = semester, Status = "Enrolled" };
            await _enrollmentRepository.AddEnrollmentAsync(enrollment);
            return true;
        }

        public async Task<bool> DropCourseAsync(string studentId, int enrollmentId)
        {
            var enrollment = await _enrollmentRepository.GetEnrollmentByIdAsync(enrollmentId);
            if (enrollment == null || enrollment.StudentId != studentId || enrollment.Status != "Enrolled") return false;
            await _enrollmentRepository.DeleteEnrollmentAsync(enrollmentId);
            return true;
        }

        public async Task<IEnumerable<EnrollmentDto>> GetTimetableAsync(string studentId, string semester)
        {
            return (await _enrollmentRepository.GetEnrollmentsByStudentAndSemesterAsync(studentId, semester)).Select(MapToEnrollmentDto);
        }

        public async Task<IEnumerable<EnrollmentDto>> GetAcademicRecordsAsync(string studentId)
        {
            return (await _enrollmentRepository.GetEnrollmentsByStudentAsync(studentId)).Select(MapToEnrollmentDto);
        }

        public async Task<IEnumerable<FeeDto>> GetFeeInformationAsync(string studentId)
        {
            return (await _feeRepository.GetFeesByStudentAsync(studentId)).Select(f => new FeeDto { FeeId = f.FeeId, StudentId = f.StudentId, Semester = f.Semester, Amount = f.Amount, IsPaid = f.IsPaid });
        }

        public async Task<bool> MarkFeeAsPaidAsync(string studentId, int feeId)
        {
            var fee = await _feeRepository.GetFeeByIdAsync(feeId);
            if (fee == null || fee.StudentId != studentId || fee.IsPaid) return false;
            fee.IsPaid = true;
            await _feeRepository.UpdateFeeAsync(fee);
            return true;
        }

        public async Task<bool> HasFeeHoldsAsync(string studentId)
        {
            return (await _feeRepository.GetUnpaidFeesByStudentAsync(studentId)).Any();
        }

        public async Task<DegreeProgressDto> GetDegreeProgressAsync(string studentId)
        {
            var student = await _studentRepository.GetStudentByIdAsync(studentId);
            var programVersion = await _programVersionRepository.GetProgramVersionByIdAsync(student.ProgramVersionId);
            var requiredCourses = await _programCoursesRepository.GetProgramCoursesByProgramVersionAsync(programVersion.ProgramVersionId);
            var requiredCourseIds = requiredCourses.Select(pc => pc.CourseId).ToList();
            var completedCourseIds = (await _enrollmentRepository.GetEnrollmentsByStudentAndStatusAsync(studentId, "Completed")).Select(e => e.CourseId);
            var completedRequiredCourseIds = completedCourseIds.Intersect(requiredCourseIds).ToList();
            var completedCredits = await _courseRepository.GetTotalCreditsByCourseIdsAsync(completedRequiredCourseIds);
            var totalCreditsRequired = programVersion.TotalCreditsRequired;
            return new DegreeProgressDto { CompletionPercentage = (double)completedCredits / totalCreditsRequired * 100, CompletedCredits = completedCredits, TotalCreditsRequired = totalCreditsRequired };
        }

        public async Task<IEnumerable<PrerequisiteNode>> GetPrerequisiteGraphAsync(string studentId, int courseId)
        {
            var completedCourseIds = (await _enrollmentRepository.GetEnrollmentsByStudentAndStatusAsync(studentId, "Completed")).Select(e => e.CourseId).ToHashSet();
            return new List<PrerequisiteNode> { await BuildPrerequisiteNode(courseId, completedCourseIds) };
        }

        private async Task<PrerequisiteNode> BuildPrerequisiteNode(int courseId, HashSet<int> completedCourseIds)
        {
            var course = await _courseRepository.GetCourseByIdAsync(courseId);
            if (course == null) throw new KeyNotFoundException("Course not found");
            var node = new PrerequisiteNode { Course = MapToCourseDto(course), IsMet = completedCourseIds.Contains(courseId) };
            var prereqs = await _prerequisiteRepository.GetPrerequisitesForCourseAsync(courseId);
            foreach (var prereq in prereqs)
                node.Prerequisites.Add(await BuildPrerequisiteNode(prereq.PrerequisiteCourseId, completedCourseIds));
            return node;
        }

        public async Task<ProgramVersionDto> GetStudentProgramAsync(string studentId)
        {
            var student = await _studentRepository.GetStudentByIdAsync(studentId);
            if (student == null) throw new KeyNotFoundException("Student not found");
            var pv = await _programVersionRepository.GetProgramVersionByIdAsync(student.ProgramVersionId);
            return new ProgramVersionDto { ProgramVersionId = pv.ProgramVersionId, ProgramId = pv.ProgramId, HandbookYear = pv.HandbookYear, TotalCreditsRequired = pv.TotalCreditsRequired };
        }

        public async Task<int> GetCurrentlyEnrolledCoursesAsync(string studentId, string semester)
        {
            return (await _enrollmentRepository.GetEnrollmentsByStudentAndSemesterAsync(studentId, semester)).Count(e => e.Status == "Enrolled");
        }

        public async Task<int> GetCoursesCompletedInCurrentYearAsync(string studentId)
        {
            var year = DateTime.Now.Year;
            return (await _enrollmentRepository.GetEnrollmentsByStudentAsync(studentId)).Count(e => e.Status == "Completed" && e.Semester.Contains(year.ToString()));
        }

        public async Task<int> GetTotalCoursesCompletedAsync(string studentId)
        {
            return (await _enrollmentRepository.GetEnrollmentsByStudentAndStatusAsync(studentId, "Completed")).Count();
        }

        public async Task<double> GetEnrollmentProgressAsync(string studentId)
        {
            var progress = await GetDegreeProgressAsync(studentId);
            return progress.CompletionPercentage;
        }

        public async Task<Dictionary<string, double>> GetGpaBySemesterAsync(string studentId)
        {
            var gradePoints = new Dictionary<string, double> { { "A+", 4.3 }, { "A", 4.0 }, { "A-", 3.7 }, { "B+", 3.3 }, { "B", 3.0 }, { "B-", 2.7 }, { "C+", 2.3 }, { "C", 2.0 } };
            var enrollments = await _enrollmentRepository.GetEnrollmentsByStudentAsync(studentId);
            return enrollments.Where(e => e.Grade != null).GroupBy(e => e.Semester).ToDictionary(
                g => g.Key,
                g => g.Average(e => gradePoints.TryGetValue(e.Grade, out var gp) ? gp : 0));
        }

        private StudentDto MapToStudentDto(Student student) => new StudentDto { StudentId = student.StudentId, LastName = student.LastName, FirstName = student.FirstName, DOB = student.DOB, Email = student.Email, Phone = student.Phone, ProgramVersionId = student.ProgramVersionId, EnrollmentYear = student.EnrollmentYear };
        private CourseDto MapToCourseDto(Course course) => new CourseDto { CourseId = course.CourseId, Code = course.Code, Name = course.Name, Credits = course.Credits, SemesterOffered = course.SemesterOffered };
        private EnrollmentDto MapToEnrollmentDto(Enrollment enrollment) => new EnrollmentDto { EnrollmentId = enrollment.EnrollmentId, StudentId = enrollment.StudentId, CourseId = enrollment.CourseId, Semester = enrollment.Semester, Status = enrollment.Status, Grade = enrollment.Grade };
    }
}