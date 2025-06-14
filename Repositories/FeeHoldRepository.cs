using ENROLLMENTSYSTEMBACKEND.Models;
using ENROLLMENTSYSTEMBACKEND.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public class FeeHoldRepository : IFeeHoldRepository
    {
        private readonly EnrollmentInformationDbContext _context;

        public FeeHoldRepository(EnrollmentInformationDbContext context)
        {
            _context = context;
        }

        public async Task<List<FeeHold>> GetFeeHoldsByStudentIdAsync(string studentId)
        {
            return await _context.FeeHolds.Where(fh => fh.StudentId == studentId).ToListAsync();
        }
    }
}