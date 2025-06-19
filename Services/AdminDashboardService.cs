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
            try
            {
                var pendingRequests = await _pendingRequestRepository.GetPendingRequestsAsync();
                var enrollments = await _enrollmentRepository.GetAllEnrollmentsAsync();
                
                var totalStudents = enrollments.Select(e => e.StudentId).Distinct().Count();
                var totalEnrollments = enrollments.Count;
                var completedEnrollments = enrollments.Count(e => e.Status == "Completed");
                var pendingRequestsCount = pendingRequests?.Count ?? 0;

                return new DashboardMetricsDto
                {
                    TotalStudents = totalStudents,
                    TotalEnrollments = totalEnrollments,
                    CompletedEnrollments = completedEnrollments,
                    PendingRequests = pendingRequestsCount,
                    CompletionRate = totalEnrollments > 0 ? (double)completedEnrollments / totalEnrollments * 100 : 0,
                    LastUpdated = DateTime.UtcNow
                };
            }
            catch (Exception)
            {
                // Return default metrics if there's an error
                return new DashboardMetricsDto
                {
                    TotalStudents = 0,
                    TotalEnrollments = 0,
                    CompletedEnrollments = 0,
                    PendingRequests = 0,
                    CompletionRate = 0,
                    LastUpdated = DateTime.UtcNow
                };
            }
        }

        public async Task<List<PendingRequestDto>> GetPendingRequestsAsync()
        {
            try
            {
                var pendingRequests = await _pendingRequestRepository.GetPendingRequestsAsync();
                if (pendingRequests == null || !pendingRequests.Any())
                {
                    return new List<PendingRequestDto>();
                }

                return pendingRequests.Select(pr => new PendingRequestDto
                {
                    RequestId = pr.Id,
                    StudentId = pr.StudentId,
                    CourseId = pr.CourseCode,
                    RequestType = pr.RequestType,
                    RequestDate = pr.Date,
                    Status = "Pending",
                    Priority = GetRequestPriority(pr.RequestType)
                }).OrderByDescending(r => r.Priority).ThenBy(r => r.RequestDate).ToList();
            }
            catch (Exception)
            {
                return new List<PendingRequestDto>();
            }
        }

        private int GetRequestPriority(string requestType)
        {
            return requestType?.ToLower() switch
            {
                "enrollment" => 3,
                "grade_change" => 2,
                "withdrawal" => 1,
                _ => 0
            };
        }

        public async Task<List<EnrollmentDataDto>> GetEnrollmentDataAsync()
        {
            try
            {
                var enrollmentCounts = await _enrollmentRepository.GetEnrollmentCountsBySemesterAsync();
                if (enrollmentCounts == null || !enrollmentCounts.Any())
                {
                    return new List<EnrollmentDataDto>();
                }

                return enrollmentCounts.Select(ec => new EnrollmentDataDto
                {
                    Semester = ec.Item1 ?? "Unknown",
                    EnrollmentCount = ec.Item2,
                    ActiveEnrollments = ec.Item2, // Assuming all are active for now
                    CompletedEnrollments = 0 // This would need additional data
                }).OrderBy(ed => ed.Semester).ToList();
            }
            catch (Exception)
            {
                return new List<EnrollmentDataDto>();
            }
        }

        public async Task<List<CompletionRateDataDto>> GetCompletionRateDataAsync()
        {
            try
            {
                var enrollments = await _enrollmentRepository.GetAllEnrollmentsAsync();
                if (enrollments == null || !enrollments.Any())
                {
                    return new List<CompletionRateDataDto>();
                }

                var groupedBySemester = enrollments.GroupBy(e => e.Semester ?? "Unknown");
                var completionRates = new List<CompletionRateDataDto>();

                foreach (var group in groupedBySemester)
                {
                    var total = group.Count();
                    var completed = group.Count(e => e.Status == "Completed" || (!string.IsNullOrEmpty(e.Grade) && e.Grade != "F"));
                    var failed = group.Count(e => e.Grade == "F");
                    var inProgress = group.Count(e => e.Status == "Enrolled");

                    var rate = total == 0 ? 0 : Math.Round((double)completed / total * 100, 2);

                    completionRates.Add(new CompletionRateDataDto
                    {
                        Semester = group.Key,
                        CompletionRate = rate,
                        TotalEnrollments = total,
                        CompletedEnrollments = completed,
                        FailedEnrollments = failed,
                        InProgressEnrollments = inProgress
                    });
                }

                return completionRates.OrderBy(cr => cr.Semester).ToList();
            }
            catch (Exception)
            {
                return new List<CompletionRateDataDto>();
            }
        }
    }
}
