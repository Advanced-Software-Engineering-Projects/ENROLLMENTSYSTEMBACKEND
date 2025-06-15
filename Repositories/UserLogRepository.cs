using Microsoft.EntityFrameworkCore;
using ENROLLMENTSYSTEMBACKEND.Models;
using ENROLLMENTSYSTEMBACKEND.Data;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public class UserLogRepository : IUserLogRepository
    {
        private readonly EnrollmentInformationDbContext _context;

        public UserLogRepository(EnrollmentInformationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserLog>> GetAllUserLogsAsync()
        {
            return await _context.UserLogs
                .Include(ul => ul.User)
                .OrderByDescending(ul => ul.UserLogTimeStamp)
                .ToListAsync();
        }

        public async Task<UserLog> GetUserLogByIdAsync(int id)
        {
            return await _context.UserLogs
                .Include(ul => ul.User)
                .FirstOrDefaultAsync(ul => ul.UserLogId == id);
        }

        public async Task<IEnumerable<UserLog>> GetUserLogsByUserIdAsync(string userId)
        {
            return await _context.UserLogs
                .Include(ul => ul.User)
                .Where(ul => ul.UserId == userId)
                .OrderByDescending(ul => ul.UserLogTimeStamp)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserLog>> GetUserLogsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.UserLogs
                .Include(ul => ul.User)
                .Where(ul => ul.UserLogTimeStamp >= startDate && ul.UserLogTimeStamp <= endDate)
                .OrderByDescending(ul => ul.UserLogTimeStamp)
                .ToListAsync();
        }

        public async Task<UserLog> CreateUserLogAsync(UserLog userLog)
        {
            _context.UserLogs.Add(userLog);
            await _context.SaveChangesAsync();
            return userLog;
        }
    }
}