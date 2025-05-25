using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public interface ISystemConfigRepository
    {
        Task<SystemConfig> GetConfigByKeyAsync(string key);
        Task AddConfigAsync(SystemConfig config);
        Task UpdateConfigAsync(SystemConfig config);
    }
}