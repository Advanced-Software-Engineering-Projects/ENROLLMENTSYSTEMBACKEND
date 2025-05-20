using Microsoft.EntityFrameworkCore;
using ENROLLMENTSYSTEMBACKEND.Data;
using ENROLLMENTSYSTEMBACKEND.IRepositories;
using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public class FeeRepository : IFeeRepository
    {
        private readonly FinancialAndAdminDbContext _context;

        public FeeRepository(FinancialAndAdminDbContext context) => _context = context;

        public async Task<IEnumerable<Fee>> GetFeesByStudentAsync(string studentId) => await _context.Fees.Where(f => f.StudentId == studentId).ToListAsync();
        public async Task<IEnumerable<Fee>> GetUnpaidFeesByStudentAsync(string studentId) => await _context.Fees.Where(f => f.StudentId == studentId && !f.IsPaid).ToListAsync();
        public async Task<Fee> GetFeeByIdAsync(int feeId) => await _context.Fees.FindAsync(feeId);
        public async Task UpdateFeeAsync(Fee fee) { _context.Fees.Update(fee); await _context.SaveChangesAsync(); }
    }
}