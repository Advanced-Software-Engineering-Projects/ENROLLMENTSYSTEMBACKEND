using ENROLLMENTSYSTEMBACKEND.Models;
using ENROLLMENTSYSTEMBACKEND.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
        public class ServiceHoldRepository : IServiceHoldRepository
        {
            private readonly EnrollmentInformationDbContext _context;

            public ServiceHoldRepository(EnrollmentInformationDbContext context)
            {
                _context = context;
            }

            public async Task<List<Student>> GetAllStudentsAsync()
            {
                return await _context.Students.ToListAsync();
            }

            public async Task<Student> GetStudentByIdAsync(string studentId)
            {
                return await _context.Students
                    .FirstOrDefaultAsync(s => s.Id == studentId);
            }

            public async Task<List<ServiceHold>> GetHoldsByStudentIdAsync(string studentId)
            {
                return await _context.ServiceHolds
                    .Where(sh => sh.StudentId == studentId)
                    .ToListAsync();
            }

            public async Task<ServiceHold> AddHoldAsync(ServiceHold hold)
            {
                _context.ServiceHolds.Add(hold);
                await _context.SaveChangesAsync();
                return hold;
            }

            public async Task<bool> RemoveHoldAsync(string holdId)
            {
                var hold = await _context.ServiceHolds.FindAsync(holdId);
                if (hold == null)
                {
                    return false;
                }
                _context.ServiceHolds.Remove(hold);
                await _context.SaveChangesAsync();
                return true;
            }
        }
}