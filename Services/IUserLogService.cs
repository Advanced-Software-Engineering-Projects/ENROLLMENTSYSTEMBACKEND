using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public interface IUserLogService
    {
        Task<IEnumerable<UserLogDto>> GetAllUserLogsAsync();
        Task<UserLogDto?> GetUserLogByIdAsync(int id);
        Task<IEnumerable<UserLogDto>> GetUserLogsByUserIdAsync(string userId);
        Task<IEnumerable<UserLogDto>> GetUserLogsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<UserLogDto> CreateUserLogAsync(string userId, string activity);
    }
}