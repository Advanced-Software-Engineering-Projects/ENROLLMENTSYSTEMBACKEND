using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public interface IFeeRepository
    {
        Task<List<Fee>> GetByStudentIdAsync(string studentId);
        Task<List<Fee>> GetAllAsync();
    }
}