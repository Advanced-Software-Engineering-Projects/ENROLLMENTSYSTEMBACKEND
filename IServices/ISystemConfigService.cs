using ENROLLMENTSYSTEMBACKEND.DTOs;

namespace ENROLLMENTSYSTEMBACKEND.IServices
{
    public interface ISystemConfigService
    {
        Task<string> GetConfigValueAsync(string key);
        Task<IEnumerable<SystemConfigDto>> GetAllConfigsAsync();
        Task<SystemConfigDto> AddConfigAsync(SystemConfigDto configDto);
        Task<SystemConfigDto> UpdateConfigAsync(int configId, SystemConfigDto configDto);
        Task<bool> DeleteConfigAsync(int configId);
    }
}