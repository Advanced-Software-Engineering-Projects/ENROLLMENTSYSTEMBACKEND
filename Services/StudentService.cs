using ENROLLMENTSYSTEMBACKEND.Repositories;
using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Models;
using StudentSystemBackend.Repositories;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;

        public StudentService(IStudentRepository studentRepository, IEnrollmentRepository enrollmentRepository)
        {
            _studentRepository = studentRepository;
            _enrollmentRepository = enrollmentRepository;
        }

        public async Task<StudentDashboardDto> GetDashboardDataAsync(string studentId)
        {
            var student = await _studentRepository.GetByIdAsync(studentId);
            if (student == null) throw new KeyNotFoundException("Student not found");

            var enrollments = await _enrollmentRepository.GetEnrollmentsByStudentAsync(studentId);
            var enrolledCourses = enrollments
             .Where(e => e.Status == "Enrolled")
             .Select(e => new CourseDto
             {
                 CourseId = e.CourseId,
                 Code = e.Course.Code,
                 Name = e.Course.Name,
                 Credits = e.Course.Credits,
                 SemesterOffered = e.Course.SemesterOffered
             }).ToList();

            var completedCourses = enrollments
                .Where(e => e.Status == "Completed" && e.Grade != null)
                .ToList();

            var gpa = CalculateGpa(completedCourses);
            var gpaTrend = CalculateGpaTrend(completedCourses);

            return new StudentDashboardDto
            {
                EnrolledCourses = enrolledCourses,
                CompletedCoursesCount = completedCourses.Count,
                Gpa = gpa,
                GpaTrend = gpaTrend
            };
        }

        public async Task<List<GradeDto>> GetGradesAsync(string studentId)
        {
            var enrollments = await _enrollmentRepository.GetEnrollmentsByStudentAsync(studentId);
            return enrollments.Select(e => new GradeDto
            {
                CourseCode = e.Course.Code,
                CourseName = e.Course.Name,
                Semester = e.Semester,
                Grade = e.Grade ?? "Not graded"
            }).ToList();
        }

        public async Task<List<TimetableDto>> GetTimetableAsync(string studentId)
        {
            var enrollments = await _enrollmentRepository.GetEnrollmentsByStudentAsync(studentId);
            // Simulated timetable data (in a real scenario, this would come from a database table)
            return enrollments.Select(e => new TimetableDto
            {
                Course = e.Course.Code,
                CourseName = e.Course.Name,
                Schedule = "09:00-10:00",
                Venue = "Room A101",
                Date = "2025-03-03",
                Day = "Monday",
                Activity = "Lecture"
            }).ToList();
        }

        public async Task<StudentProfileDto> GetProfileAsync(string studentId)
        {
            var student = await _studentRepository.GetByIdAsync(studentId);
            if (student == null) throw new KeyNotFoundException("Student not found");

            return new StudentProfileDto
            {
                FirstName = student.FirstName,
                MiddleName = "",
                LastName = student.LastName,
                StudentId = student.StudentId,
                Dob = student.DOB.ToString("yyyy-MM-dd"),
                Email = student.Email,
                Phone = student.Phone,
                Gender = "Not specified",
                Citizenship = "Not specified",
                Program = "Bachelor of Science", // Simplified
                StudentLevel = "Sophomore",
                StudentCampus = "Main Campus",
                ExamSite = "Campus Testing Center",
                MajorType = "Single",
                Major1 = "Computer Science",
                Major2 = "",
                Avatar = "https://via.placeholder.com/150"
            };
        }

        public async Task<StudentProfileDto> UpdateProfileAsync(string studentId, StudentProfileDto profile)
        {
            var student = await _studentRepository.GetByIdAsync(studentId);
            if (student == null) throw new KeyNotFoundException("Student not found");

            student.FirstName = profile.FirstName;
            student.LastName = profile.LastName;
            student.Email = profile.Email;
            student.Phone = profile.Phone;
            await _studentRepository.UpdateAsync(student);

            return profile;
        }

        public async Task<AcademicRecordsDto> GetAcademicRecordsAsync(string studentId)
        {
            var student = await _studentRepository.GetByIdAsync(studentId);
            if (student == null) throw new KeyNotFoundException("Student not found");

            var enrollments = await _enrollmentRepository.GetEnrollmentsByStudentAsync(studentId);
            var completedCourses = enrollments.Where(e => e.Status == "Completed" && e.Grade != null).ToList();
            var gpa = CalculateGpa(completedCourses);

            return new AcademicRecordsDto
            {
                StudentId = student.StudentId,
                StudentEmail = student.Email,
                Gpa = gpa,
                Enrollments = enrollments.Select(e => new EnrollmentDto
                {
                    EnrollmentId = e.EnrollmentId.ToString(),
                    CourseCode = e.Course.Code,
                    CourseName = e.Course.Name,
                    Semester = e.Semester,
                    Grade = e.Grade ?? "IP",
                    Year = int.Parse(e.Semester.Split(" ")[2]) // Extract year from "Semester 1 2023"
                }).OrderBy(e => e.Semester).ToList()
            };
        }

        private double CalculateGpa(List<Enrollment> completedCourses)
        {
            if (completedCourses.Count == 0) return 0.0;

            var gradePoints = new Dictionary<string, double>
            {
                { "A+", 4.3 }, { "A", 4.0 }, { "A-", 3.7 },
                { "B+", 3.3 }, { "B", 3.0 }, { "B-", 2.7 },
                { "C+", 2.3 }, { "C", 2.0 }, { "C-", 1.7 },
                { "D", 1.0 }, { "E", 0.0 }
            };

            double totalPoints = 0;
            int totalCourses = 0;

            foreach (var enrollment in completedCourses)
            {
                if (gradePoints.ContainsKey(enrollment.Grade))
                {
                    totalPoints += gradePoints[enrollment.Grade];
                    totalCourses++;
                }
            }

            return totalCourses > 0 ? Math.Round(totalPoints / totalCourses, 2) : 0.0;
        }

        private List<GpaTrendDto> CalculateGpaTrend(List<Enrollment> completedCourses)
        {
            if (completedCourses.Count == 0) return new List<GpaTrendDto>();

            var gpaTrend = new List<GpaTrendDto>();
            var gradePoints = new Dictionary<string, double>
            {
                { "A+", 4.3 }, { "A", 4.0 }, { "A-", 3.7 },
                { "B+", 3.3 }, { "B", 3.0 }, { "B-", 2.7 },
                { "C+", 2.3 }, { "C", 2.0 }, { "C-", 1.7 },
                { "D", 1.0 }, { "E", 0.0 }
            };

            var groupedBySemester = completedCourses
                .GroupBy(e => e.Semester)
                .OrderBy(g => g.Key);

            foreach (var semesterGroup in groupedBySemester)
            {
                double totalPoints = 0;
                int totalCourses = 0;

                foreach (var enrollment in semesterGroup)
                {
                    if (gradePoints.ContainsKey(enrollment.Grade))
                    {
                        totalPoints += gradePoints[enrollment.Grade];
                        totalCourses++;
                    }
                }

                if (totalCourses > 0)
                {
                    gpaTrend.Add(new GpaTrendDto
                    {
                        Semester = semesterGroup.Key,
                        Gpa = Math.Round(totalPoints / totalCourses, 2)
                    });
                }
            }

            return gpaTrend;
        }
    }
}