using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public class FeeRepository : IFeeRepository
    {
        private readonly List<Fee> _fees = new List<Fee>();

        public async Task<List<Fee>> GetFeesByStudentIdAsync(string studentId)
        {
            return await Task.FromResult(_fees.Where(f => f.StudentId == studentId).ToList());
        }

        public async Task<Fee> GetFeeByIdAsync(string feeId)
        {
            return await Task.FromResult(_fees.FirstOrDefault(f => f.Id == feeId));
        }

        public async Task UpdateFeeAsync(Fee fee)
        {
            var existingFee = _fees.FirstOrDefault(f => f.Id == fee.Id);
            if (existingFee != null)
            {
                existingFee.IsPaid = fee.IsPaid;
            }
            await Task.CompletedTask;
        }
    }
}