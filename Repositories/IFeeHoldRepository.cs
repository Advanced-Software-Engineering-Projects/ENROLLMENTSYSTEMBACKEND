using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public interface IFeeHoldRepository
    {
        Task<List<FeeHold>> GetFeeHoldsByStudentIdAsync(string studentId);
    }
}
