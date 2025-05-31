using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public interface IDashboardService
    {
        Task<List<EnrolledCourse>> GetEnrolledCoursesAsync(string studentId);
        Task<int> GetCompletedCoursesCurrentYearAsync(string studentId);
        Task<int> GetTotalCompletedCoursesAsync(string studentId);
        Task<List<GpaData>> GetGpaDataAsync(string studentId);
        Task<int> GetRegisteredStudentsCountAsync();
        Task<int> GetActiveCoursesCountAsync();
        Task<int> GetPendingApprovalsCountAsync();
        Task<List<PendingRequest>> GetPendingRequestsAsync();
        Task<List<EnrollmentData>> GetEnrollmentDataAsync();
        Task<List<CompletionRateData>> GetCompletionRateDataAsync();
    }
}
