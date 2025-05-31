using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public interface IHoldRepository
    {
        Task<List<Hold>> GetHoldsAsync(string? studentId);
        Task<Hold> GetHoldByIdAsync(string id);
        Task AddHoldAsync(Hold hold);
        Task RemoveHoldAsync(string id);
    }
}
