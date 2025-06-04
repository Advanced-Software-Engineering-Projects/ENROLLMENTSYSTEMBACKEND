using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly IPendingRequestRepository _pendingRequestRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IGradeService _gradeService;

        public AdminDashboardService(
            IPendingRequestRepository pendingRequestRepository,
            IEnrollmentRepository enrollmentRepository,
            IGradeService gradeService)
        {
            _pendingRequestRepository = pendingRequestRepository;
            _enrollmentRepository = enrollmentRepository;
            _gradeService = gradeService;
        }

        public async Task<DashboardMetricsDto> GetDashboardMetricsAsync()
        {
            // Placeholder implementation - can be extended to aggregate real data
            return await Task.FromResult(new DashboardMetricsDto
            {
                RequestId = "SampleRequestId",
                StudentId = "SampleStudentId",
                CourseId = "SampleCourseId",
                RequestDate = DateTime.UtcNow
            });
        }

        public async Task<List<PendingRequestDto>> GetPendingRequestsAsync()
        {
            var pendingRequests = await _pendingRequestRepository.GetPendingRequestsAsync();
            return pendingRequests.Select(pr => new PendingRequestDto
            {
                RequestId = pr.Id,
                StudentId = pr.StudentId,
                CourseId = pr.CourseCode,
                RequestDate = pr.Date
            }).ToList();
        }

        public async Task<List<EnrollmentDataDto>> GetEnrollmentDataAsync()
        {
            var enrollmentCounts = await _enrollmentRepository.GetEnrollmentCountsBySemesterAsync();
            return enrollmentCounts.Select(ec => new EnrollmentDataDto
            {
                Semester = ec.Item1,
                EnrollmentCount = ec.Item2
            }).ToList();
        }

        public async Task<List<CompletionRateDataDto>> GetCompletionRateDataAsync()
        {
            var enrollments = await _enrollmentRepository.GetAllEnrollmentsAsync();
            var groupedBySemester = enrollments.GroupBy(e => e.Semester);

            var completionRates = new List<CompletionRateDataDto>();

            foreach (var group in groupedBySemester)
            {
                var total = group.Count();
                var completed = group.Count(e => !string.IsNullOrEmpty(e.Grade) && e.Grade != "F");

                var rate = total == 0 ? 0 : (double)completed / total;

                completionRates.Add(new CompletionRateDataDto
                {
                    Semester = group.Key,
                    CompletionRate = rate
                });
            }

            return completionRates;
        }
    }
}
