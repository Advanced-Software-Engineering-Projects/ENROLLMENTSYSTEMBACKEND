using ENROLLMENTSYSTEMBACKEND.DTOs;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public interface IFeeService
    {
        Task<List<FeeDto>> GetFeesByStudentIdAsync(string studentId);
    }
}
