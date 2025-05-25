using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public interface IUserActivityService
    {
        public interface IUserActivityService
        {
            Task LogActivityAsync(string userId, string action, string details);
            Task<List<UserActivity>> GetActivitiesAsync(string userId);
        }
    }
}
