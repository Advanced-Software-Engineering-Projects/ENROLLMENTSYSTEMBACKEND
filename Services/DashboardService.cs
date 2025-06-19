using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Models;
using ENROLLMENTSYSTEMBACKEND.Repositories;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IPendingRequestRepository _pendingRequestRepository;

        public DashboardService(
            ICourseRepository courseRepository,
            IStudentRepository studentRepository,
            IEnrollmentRepository enrollmentRepository,
            IPendingRequestRepository pendingRequestRepository)
        {
            _courseRepository = courseRepository;
            _studentRepository = studentRepository;
            _enrollmentRepository = enrollmentRepository;
            _pendingRequestRepository = pendingRequestRepository;
        }

        public async Task<List<EnrolledCourse>> GetEnrolledCoursesAsync(string studentId)
        {
            var enrollments = await _enrollmentRepository.GetEnrollmentsByStudentIdAsync(studentId);
            if (enrollments == null || !enrollments.Any())
            {
                return new List<EnrolledCourse>();
            }

            var enrolledCourses = enrollments.Where(e => e.Status == "Enrolled").ToList();
            if (!enrolledCourses.Any())
            {
                return new List<EnrolledCourse>();
            }

            var courseIds = enrolledCourses.Select(e => e.CourseId).ToList();
            var allCourses = await _courseRepository.GetAllCoursesAsync();
            
            return allCourses
                .Where(c => courseIds.Contains(c.CourseId))
                .Select(c => new EnrolledCourse
                {
                    CourseId = c.CourseId,
                    CourseCode = c.CourseCode,
                    CourseName = c.CourseName,
                    DueDate = c.DueDate.ToString("yyyy-MM-dd")
                })
                .ToList();
        }

        public async Task<int> GetCompletedCoursesCurrentYearAsync(string studentId)
        {
            var enrollments = await _enrollmentRepository.GetEnrollmentsByStudentIdAsync(studentId);
            var currentYear = DateTime.Now.Year;
            return enrollments.Count(e => e.Status == "Completed" && e.CompletionDate.HasValue && e.CompletionDate.Value.Year == currentYear);
        }

        public async Task<int> GetTotalCompletedCoursesAsync(string studentId)
        {
            var enrollments = await _enrollmentRepository.GetEnrollmentsByStudentIdAsync(studentId);
            return enrollments.Count(e => e.Status == "Completed");
        }

        public async Task<List<GpaData>> GetGpaDataAsync(string studentId)
        {
            var enrollments = await _enrollmentRepository.GetEnrollmentsByStudentIdAsync(studentId);
            var gpaData = enrollments
                .Where(e => e.Grade != null)
                .GroupBy(e => e.Semester)
                .Select(g => new GpaData
                {
                    Semester = g.Key,
                    GPA = Math.Round(g.Average(e => ConvertGradeToGpa(e.Grade)), 2)
                })
                .OrderBy(g => g.Semester)
                .ToList();
            return gpaData;
        }

        private double ConvertGradeToGpa(string grade)
        {
            return grade.ToUpper() switch
            {
                "A+" => 4.5,
                "A" => 4.0,
                "B+" => 3.5,
                "B" => 3.0,
                "C+" => 2.5,
                "C" => 2.0,
                "D" => 1.5,
                "E" => 1.0,
                "F" => 0.0,
                _ => 0.0,
            };
        }

        public async Task<int> GetRegisteredStudentsCountAsync()
        {
            return await _studentRepository.GetRegisteredStudentsCountAsync();
        }

        public async Task<int> GetActiveCoursesCountAsync()
        {
            var courses = await _courseRepository.GetAllCoursesAsync();
            return courses.Count(c => c.IsActive);
        }

        public async Task<int> GetPendingApprovalsCountAsync()
        {
            return await _pendingRequestRepository.GetPendingApprovalsCountAsync();
        }

        public async Task<List<PendingRequest>> GetPendingRequestsAsync()
        {
            return await _pendingRequestRepository.GetPendingRequestsAsync();
        }

        public async Task<List<EnrollmentData>> GetEnrollmentDataAsync()
        {
            var enrollments = await _enrollmentRepository.GetAllEnrollmentsAsync();
            var enrollmentData = enrollments
                .GroupBy(e => e.Semester)
                .Select(g => new EnrollmentData
                {
                    Semester = g.Key,
                    Students = g.Select(e => e.StudentId).Distinct().Count()
                })
                .ToList();
            return enrollmentData;
        }

        public async Task<List<CompletionRateData>> GetCompletionRateDataAsync()
        {
            var enrollments = await _enrollmentRepository.GetAllEnrollmentsAsync();
            if (enrollments == null || !enrollments.Any())
            {
                return new List<CompletionRateData>();
            }

            var completionRateData = enrollments
                .GroupBy(e => e.Semester)
                .Select(g =>
                {
                    var totalCount = g.Count();
                    var completedCount = g.Count(e => e.Status == "Completed");
                    var rate = totalCount > 0 ? (int)((double)completedCount / totalCount * 100) : 0;

                    return new CompletionRateData
                    {
                        Semester = g.Key,
                        Rate = rate,
                        TotalEnrollments = totalCount,
                        CompletedEnrollments = completedCount
                    };
                })
                .OrderBy(d => d.Semester)
                .ToList();

            return completionRateData;
        }
    }
}
