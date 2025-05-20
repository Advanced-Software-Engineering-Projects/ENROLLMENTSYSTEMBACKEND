using Newtonsoft.Json;
using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.IRepositories;
using ENROLLMENTSYSTEMBACKEND.IServices;
using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public class UserActivityService : IUserActivityService
    {
        private readonly IUserActivityRepository _userActivityRepository;

        public UserActivityService(IUserActivityRepository userActivityRepository) => _userActivityRepository = userActivityRepository;

        public async Task LogActivityAsync(string userId, string userType, string action, object details)
        {
            await _userActivityRepository.AddActivityAsync(new UserActivity { UserId = userId, UserType = userType, Action = action, Timestamp = DateTime.UtcNow, Details = JsonConvert.SerializeObject(details) });
        }

        public async Task<IEnumerable<UserActivityDto>> GetUserActivitiesAsync(string userId)
        {
            return (await _userActivityRepository.GetActivitiesByUserIdAsync(userId)).Select(a => new UserActivityDto { ActivityId = a.ActivityId, UserId = a.UserId, UserType = a.UserType, Action = a.Action, Timestamp = a.Timestamp, Details = a.Details });
        }

        public async Task<IEnumerable<UserActivityDto>> GetAuditLogsAsync(string? userId, DateTime? startDate, DateTime? endDate)
        {
            return (await _userActivityRepository.GetActivitiesAsync(userId, startDate, endDate)).Select(a => new UserActivityDto { ActivityId = a.ActivityId, UserId = a.UserId, UserType = a.UserType, Action = a.Action, Timestamp = a.Timestamp, Details = a.Details });
        }
    }
}