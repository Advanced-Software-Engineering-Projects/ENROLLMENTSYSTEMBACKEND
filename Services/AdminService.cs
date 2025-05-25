using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Models;
using ENROLLMENTSYSTEMBACKEND.Repositories;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _repo;

        public AdminService(IAdminRepository repo)
        {
            _repo = repo;
        }

        public async Task<AdminDashboardDto> GetDashboardDataAsync()
        {
            var registeredStudents = await _repo.GetRegisteredStudentsCountAsync();
            var activeCourses = await _repo.GetActiveCoursesCountAsync();
            var pendingApprovals = await _repo.GetPendingApprovalsCountAsync();
            var pendingRequests = await _repo.GetPendingRequestsAsync();
            var enrollmentData = await _repo.GetEnrollmentDataAsync();
            var completionRateData = await _repo.GetCompletionRateDataAsync();

            return new AdminDashboardDto
            {
                RegisteredStudents = registeredStudents,
                ActiveCourses = activeCourses,
                PendingApprovals = pendingApprovals,
                PendingRequests = pendingRequests.Select(r => new PendingRequestDto
                {
                    Id = r.RequestId,
                    StudentId = r.StudentId,
                    CourseCode = r.CourseCode,
                    RequestType = r.RequestType,
                    Date = r.Date.ToString("yyyy-MM-dd")
                }).ToList(),
                EnrollmentData = enrollmentData.Select(e => new EnrollmentDataDto
                {
                    Semester = e.Semester,
                    Students = e.Students
                }).ToList(),
                CompletionRateData = completionRateData.Select(c => new CompletionRateDto
                {
                    Semester = c.Course?.SemesterOffered ?? "Unknown", // Use SemesterOffered from Course
                    Rate = (double)c.Rate
                }).ToList()
            };
        }

        public async Task<List<StudentDto>> GetAllStudentsAsync()
        {
            var students = await _repo.GetAllStudentsAsync();
            return students.Select(s => new StudentDto
            {
                StudentId = s.StudentId,
                FirstName = s.FirstName,
                LastName = s.LastName
            }).ToList();
        }

        public async Task<List<HoldDto>> GetHoldsAsync(string studentId)
        {
            var holds = await _repo.GetHoldsByStudentIdAsync(studentId);
            return holds.Select(h => new HoldDto
            {
                HoldId = h.HoldId,
                StudentId = h.StudentId,
                Reason = h.Reason
            }).ToList();
        }

        public async Task AddHoldAsync(HoldDto holdDto)
        {
            var hold = new Hold
            {
                HoldId = holdDto.HoldId,
                StudentId = holdDto.StudentId,
                Reason = holdDto.Reason
            };
            await _repo.AddHoldAsync(hold);
        }

        public async Task OpenRegistrationAsync(RegistrationPeriodDto period)
        {
            var registrationPeriod = new RegistrationPeriod
            {
                StartDate = period.StartDate,
                EndDate = period.EndDate,
                IsActive = true
            };
            await _repo.SetRegistrationPeriodAsync(registrationPeriod);
        }

        public async Task CloseRegistrationAsync()
        {
            await _repo.CloseRegistrationAsync();
        }
    }
}