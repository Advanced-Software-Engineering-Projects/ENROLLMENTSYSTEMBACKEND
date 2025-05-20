using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.IRepositories
{
    public interface IFeeRepository
    {
        Task<IEnumerable<Fee>> GetFeesByStudentAsync(string studentId);
        Task<IEnumerable<Fee>> GetUnpaidFeesByStudentAsync(string studentId);
        Task<Fee> GetFeeByIdAsync(int feeId);
        Task UpdateFeeAsync(Fee fee);
    }
}
