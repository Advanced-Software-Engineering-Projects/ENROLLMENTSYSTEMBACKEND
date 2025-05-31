using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public class PendingRequestRepository : IPendingRequestRepository
    {
        private readonly List<PendingRequest> _pendingRequests = new List<PendingRequest>
        {
            new PendingRequest { Id = "R1", StudentId = "S1", CourseCode = "MATH101", RequestType = "Enrollment", Date = DateTime.Now }
        };

        public async Task<List<PendingRequest>> GetPendingRequestsAsync()
        {
            return await Task.FromResult(_pendingRequests);
        }

        public async Task<int> GetPendingApprovalsCountAsync()
        {
            return await Task.FromResult(_pendingRequests.Count);
        }
    }
}
