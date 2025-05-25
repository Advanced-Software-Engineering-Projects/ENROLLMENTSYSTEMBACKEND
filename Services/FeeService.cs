using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Repositories;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public class FeeService : IFeeService
    {
        private readonly IFeeRepository _repo;
        public FeeService(IFeeRepository repo) => _repo = repo;

        public async Task<List<FeeDto>> GetFeesByStudentIdAsync(string studentId)
        {
            var fees = await _repo.GetByStudentIdAsync(studentId);
            return fees.Select(f => new FeeDto
            {
                FeeId = f.FeeId,
                Semester = f.Semester,
                Amount = f.Amount,
                IsPaid = f.IsPaid
            }).ToList();
        }
    }
}