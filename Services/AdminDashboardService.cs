using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public class AdminDashboardService : IAdminDashboardService
    {
        public Task<DashboardMetricsDto> GetDashboardMetricsAsync()
        {
            // TODO: Implement actual logic
            throw new NotImplementedException();
        }

        public Task<List<PendingRequestDto>> GetPendingRequestsAsync()
        {
            // TODO: Implement actual logic
            throw new NotImplementedException();
        }

        public Task<List<EnrollmentDataDto>> GetEnrollmentDataAsync()
        {
            // TODO: Implement actual logic
            throw new NotImplementedException();
        }

        public Task<List<CompletionRateDataDto>> GetCompletionRateDataAsync()
        {
            // TODO: Implement actual logic
            throw new NotImplementedException();
        }
    }
}
