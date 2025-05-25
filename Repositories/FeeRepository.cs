using Microsoft.EntityFrameworkCore;
using ENROLLMENTSYSTEMBACKEND.Models;
using ENROLLMENTSYSTEMBACKEND.Data;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public class FeeRepository : IFeeRepository
    {
        private readonly FinancialAndAdminDbContext _context;

        public FeeRepository(FinancialAndAdminDbContext context)
        {
            _context = context;
        }

        public async Task<List<Fee>> GetByStudentIdAsync(string studentId)
        {
            return await _context.Fees
                .Where(f => f.StudentId == studentId)
                .ToListAsync();
        }

        public async Task<List<Fee>> GetAllAsync()
        {
            return await _context.Fees.ToListAsync();
        }
    }
}