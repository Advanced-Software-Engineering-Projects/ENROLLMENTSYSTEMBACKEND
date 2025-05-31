using ENROLLMENTSYSTEMBACKEND.DTOs;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public interface IAdminDashboardService
    {
        Task<DashboardMetricsDto> GetDashboardMetricsAsync();
        Task<List<PendingRequestDto>> GetPendingRequestsAsync();
        Task<List<EnrollmentDataDto>> GetEnrollmentDataAsync();
        Task<List<CompletionRateDataDto>> GetCompletionRateDataAsync();
    }
}
