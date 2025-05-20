using Microsoft.EntityFrameworkCore;
using ENROLLMENTSYSTEMBACKEND.Data;
using ENROLLMENTSYSTEMBACKEND.IRepositories;
using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public class UserActivityRepository : IUserActivityRepository
    {
        private readonly FinancialAndAdminDbContext _context;

        public UserActivityRepository(FinancialAndAdminDbContext context) => _context = context;

        public async Task AddActivityAsync(UserActivity activity) { await _context.UserActivities.AddAsync(activity); await _context.SaveChangesAsync(); }
        public async Task<IEnumerable<UserActivity>> GetActivitiesByUserIdAsync(string userId) => await _context.UserActivities.Where(a => a.UserId == userId).ToListAsync();
        public async Task<IEnumerable<UserActivity>> GetActivitiesAsync(string? userId, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.UserActivities.AsQueryable();
            if (!string.IsNullOrEmpty(userId)) query = query.Where(a => a.UserId == userId);
            if (startDate.HasValue) query = query.Where(a => a.Timestamp >= startDate.Value);
            if (endDate.HasValue) query = query.Where(a => a.Timestamp <= endDate.Value);
            return await query.ToListAsync();
        }
    }
}