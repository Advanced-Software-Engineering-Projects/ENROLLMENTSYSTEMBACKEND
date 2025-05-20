using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.IRepositories
{
    public interface ISystemConfigRepository
    {
        Task<string> GetConfigValueAsync(string key);
        Task<IEnumerable<SystemConfig>> GetAllConfigsAsync();
        Task AddConfigAsync(SystemConfig config);
        Task UpdateConfigAsync(SystemConfig config);
        Task DeleteConfigAsync(int configId);
    }
}
