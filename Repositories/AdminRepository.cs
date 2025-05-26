using Microsoft.EntityFrameworkCore;
using ENROLLMENTSYSTEMBACKEND.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using ENROLLMENTSYSTEMBACKEND.Data;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly FinancialAndAdminDbContext _context;

        public AdminRepository(FinancialAndAdminDbContext context)
        {
            _context = context;
        }

        // Existing methods
        public async Task<Admin> GetByUsernameAsync(string username)
        {
            return await _context.Admins.FirstOrDefaultAsync(a => a.Username == username || a.Email == username);
        }

        public async Task<int> GetRegisteredStudentsCountAsync()
        {
            return await _context.Students.CountAsync();
        }

        public async Task<int> GetActiveCoursesCountAsync()
        {
            return await _context.Courses.CountAsync(c => c.IsActive);
        }

        public async Task<int> GetPendingApprovalsCountAsync()
        {
            return await _context.PendingRequests.CountAsync(r => r.Status == "Pending");
        }

        public async Task<Admin> GetByRefreshTokenAsync(string refreshToken)
        {
            return await _context.Admins.FirstOrDefaultAsync(a => a.RefreshToken == refreshToken);
        }

        public async Task<List<PendingRequest>> GetPendingRequestsAsync()
        {
            return await _context.PendingRequests.ToListAsync();
        }

        public async Task<List<EnrollmentData>> GetEnrollmentDataAsync()
        {
            return await _context.EnrollmentData.ToListAsync();
        }

        public async Task<List<CompletionRate>> GetCompletionRateDataAsync()
        {
            return await _context.CompletionRates.ToListAsync();
        }

        public async Task<List<Student>> GetAllStudentsAsync()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<List<Hold>> GetHoldsByStudentIdAsync(string studentId)
        {
            return await _context.Holds.Where(h => h.StudentId == studentId).ToListAsync();
        }

        public async Task AddHoldAsync(Hold hold)
        {
            await _context.Holds.AddAsync(hold);
            await _context.SaveChangesAsync();
        }

        public async Task SetRegistrationPeriodAsync(RegistrationPeriod period)
        {
            var existingPeriod = await _context.RegistrationPeriods.FirstOrDefaultAsync();
            if (existingPeriod != null)
            {
                existingPeriod.StartDate = period.StartDate;
                existingPeriod.EndDate = period.EndDate;
                existingPeriod.IsActive = true;
            }
            else
            {
                await _context.RegistrationPeriods.AddAsync(period);
            }
            await _context.SaveChangesAsync();
        }

        public async Task CloseRegistrationAsync()
        {
            var period = await _context.RegistrationPeriods.FirstOrDefaultAsync();
            if (period != null)
            {
                period.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }

        // Newly added method to fix the error
        public async Task UpdateAsync(Admin admin)
        {
            _context.Admins.Update(admin);
            await _context.SaveChangesAsync();
        }
    }
}