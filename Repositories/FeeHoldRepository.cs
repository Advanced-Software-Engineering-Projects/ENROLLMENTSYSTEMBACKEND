using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public class FeeHoldRepository : IFeeHoldRepository
    {
        private readonly List<FeeHold> _feeHolds = new List<FeeHold>();

        public async Task<List<FeeHold>> GetFeeHoldsByStudentIdAsync(string studentId)
        {
            return await Task.FromResult(_feeHolds.Where(fh => fh.StudentId == studentId).ToList());
        }
    }
}
