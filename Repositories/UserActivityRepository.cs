using ENROLLMENTSYSTEMBACKEND.Data;
using ENROLLMENTSYSTEMBACKEND.Models;
using Microsoft.EntityFrameworkCore;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public class UserActivityRepository : IUserActivityRepository
    {
        private readonly FinancialAndAdminDbContext _context;

        public UserActivityRepository(FinancialAndAdminDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(UserActivity activity)
        {
            _context.UserActivities.Add(activity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<UserActivity>> GetActivitiesByUserIdAsync(string userId)
        {
            return await _context.UserActivities
                .Where(activity => activity.UserId == userId)
                .ToListAsync();
        }
    }
}