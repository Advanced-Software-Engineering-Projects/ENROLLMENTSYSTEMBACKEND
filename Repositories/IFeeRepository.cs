using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public interface IFeeRepository
    {
        Task<List<Fee>> GetFeesByStudentIdAsync(string studentId);
        Task<Fee> GetFeeByIdAsync(string feeId);
        Task UpdateFeeAsync(Fee fee);
    }
}
