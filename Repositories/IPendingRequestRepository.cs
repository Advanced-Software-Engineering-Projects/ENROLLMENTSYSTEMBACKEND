using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public interface IPendingRequestRepository
    {
        Task<List<PendingRequest>> GetPendingRequestsAsync();
        Task<int> GetPendingApprovalsCountAsync();
    }
}
