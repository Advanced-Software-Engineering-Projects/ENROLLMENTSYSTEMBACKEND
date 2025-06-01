using ENROLLMENTSYSTEMBACKEND.Models;
using ENROLLMENTSYSTEMBACKEND.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public class FeeRepository : IFeeRepository
    {
        private readonly EnrollmentInfromation _context;

        public FeeRepository(EnrollmentInfromation context)
        {
            _context = context;
        }

        public async Task<List<Fee>> GetFeesByStudentIdAsync(string studentId)
        {
            return await _context.Fees.Where(f => f.StudentId == studentId).ToListAsync();
        }

        public async Task<Fee> GetFeeByIdAsync(string feeId)
        {
            return await _context.Fees.FirstOrDefaultAsync(f => f.Id == feeId);
        }

        public async Task UpdateFeeAsync(Fee fee)
        {
            _context.Fees.Update(fee);
            await _context.SaveChangesAsync();
        }
    }
}
