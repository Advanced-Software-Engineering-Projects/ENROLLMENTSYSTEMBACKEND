using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Models;
using ENROLLMENTSYSTEMBACKEND.Repositories;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public class UserLogService : IUserLogService
    {
        private readonly IUserLogRepository _userLogRepository;
        private readonly IUserRepository _userRepository;

        public UserLogService(IUserLogRepository userLogRepository, IUserRepository userRepository)
        {
            _userLogRepository = userLogRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserLogDto>> GetAllUserLogsAsync()
        {
            var logs = await _userLogRepository.GetAllUserLogsAsync();
            return logs.Select(MapToDto);
        }

        public async Task<UserLogDto?> GetUserLogByIdAsync(int id)
        {
            var log = await _userLogRepository.GetUserLogByIdAsync(id);
            return log != null ? MapToDto(log) : null;
        }

        public async Task<IEnumerable<UserLogDto>> GetUserLogsByUserIdAsync(string userId)
        {
            var logs = await _userLogRepository.GetUserLogsByUserIdAsync(userId);
            return logs.Select(MapToDto);
        }

        public async Task<IEnumerable<UserLogDto>> GetUserLogsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var logs = await _userLogRepository.GetUserLogsByDateRangeAsync(startDate, endDate);
            return logs.Select(MapToDto);
        }

        public async Task<UserLogDto> CreateUserLogAsync(string userId, string activity)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            var userLog = new UserLog
            {
                UserId = userId,
                EmailAddress = user.Email,
                UserLogActivity = activity,
                UserLogTimeStamp = DateTime.UtcNow,
                UserProfileImagePath = "" // You might want to add user profile image path here if available
            };

            var createdLog = await _userLogRepository.CreateUserLogAsync(userLog);
            return MapToDto(createdLog);
        }

        private UserLogDto MapToDto(UserLog log)
        {
            return new UserLogDto
            {
                UserLogId = log.UserLogId,
                EmailAddress = log.EmailAddress,
                UserLogTimeStamp = log.UserLogTimeStamp,
                UserLogActivity = log.UserLogActivity,
                UserProfileImagePath = log.UserProfileImagePath
            };
        }
    }
}