using ENROLLMENTSYSTEMBACKEND.Models;
using ENROLLMENTSYSTEMBACKEND.Repositories;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public class UserActivityService : IUserActivityService
    {
        private readonly IUserActivityRepository _repository;

        public UserActivityService(IUserActivityRepository repository)
        {
            _repository = repository;
        }

        public async Task LogActivityAsync(string userId, string action, string details)
        {
            var activity = new UserActivity
            {
                UserId = userId,
                Action = action,
                Details = details,
                Timestamp = DateTime.UtcNow
            };
            await _repository.AddAsync(activity); // Fixed method name
        }

        public async Task<List<UserActivity>> GetActivitiesAsync(string userId)
        {
            // Added missing method in IUserActivityRepository
            return await _repository.GetActivitiesByUserIdAsync(userId);
        }
    }
}