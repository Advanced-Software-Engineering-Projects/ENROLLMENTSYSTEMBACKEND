using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public interface IUserActivityRepository
    {
        Task AddAsync(UserActivity activity);
        Task<List<UserActivity>> GetActivitiesByUserIdAsync(string userId);
    }
}