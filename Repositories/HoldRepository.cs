using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public class HoldRepository : IHoldRepository
    {
        private readonly List<Hold> _holds = new List<Hold>();

        public async Task<List<Hold>> GetHoldsAsync(string? studentId)
        {
            if (string.IsNullOrEmpty(studentId))
            {
                return await Task.FromResult(_holds);
            }
            return await Task.FromResult(_holds.Where(h => h.StudentId == studentId).ToList());
        }

        public async Task<Hold> GetHoldByIdAsync(string id)
        {
            var hold = _holds.FirstOrDefault(h => h.Id == id);
            if (hold == null)
            {
                throw new InvalidOperationException("Hold not found.");
            }
            return await Task.FromResult(hold);
        }

        public async Task AddHoldAsync(Hold hold)
        {
            hold.Id = Guid.NewGuid().ToString();
            hold.CreatedAt = DateTime.UtcNow;
            _holds.Add(hold);
            await Task.CompletedTask;
        }

        public async Task RemoveHoldAsync(string id)
        {
            var hold = _holds.FirstOrDefault(h => h.Id == id);
            if (hold == null)
            {
                throw new InvalidOperationException("Hold not found.");
            }
            _holds.Remove(hold);
            await Task.CompletedTask;
        }
    }
}