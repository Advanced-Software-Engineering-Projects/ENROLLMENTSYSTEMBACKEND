using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public interface IUserLogRepository
    {
        Task<IEnumerable<UserLog>> GetAllUserLogsAsync();
        Task<UserLog> GetUserLogByIdAsync(int id);
        Task<IEnumerable<UserLog>> GetUserLogsByUserIdAsync(string userId);
        Task<IEnumerable<UserLog>> GetUserLogsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<UserLog> CreateUserLogAsync(UserLog userLog);
    }
}