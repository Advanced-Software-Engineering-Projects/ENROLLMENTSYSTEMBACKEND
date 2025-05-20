using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.IRepositories;
using ENROLLMENTSYSTEMBACKEND.IServices;
using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public class SystemConfigService : ISystemConfigService
    {
        private readonly ISystemConfigRepository _systemConfigRepository;

        public SystemConfigService(ISystemConfigRepository systemConfigRepository) => _systemConfigRepository = systemConfigRepository;

        public async Task<string> GetConfigValueAsync(string key) => await _systemConfigRepository.GetConfigValueAsync(key);
        public async Task<IEnumerable<SystemConfigDto>> GetAllConfigsAsync() => (await _systemConfigRepository.GetAllConfigsAsync()).Select(c => new SystemConfigDto { ConfigId = c.ConfigId, Key = c.Key, Value = c.Value });
        public async Task<SystemConfigDto> AddConfigAsync(SystemConfigDto configDto)
        {
            var config = new SystemConfig { Key = configDto.Key, Value = configDto.Value };
            await _systemConfigRepository.AddConfigAsync(config);
            return new SystemConfigDto { ConfigId = config.ConfigId, Key = config.Key, Value = config.Value };
        }
        public async Task<SystemConfigDto> UpdateConfigAsync(int configId, SystemConfigDto configDto)
        {
            var config = (await _systemConfigRepository.GetAllConfigsAsync()).FirstOrDefault(c => c.ConfigId == configId);
            if (config == null) throw new KeyNotFoundException("Config not found");
            config.Key = configDto.Key;
            config.Value = configDto.Value;
            await _systemConfigRepository.UpdateConfigAsync(config);
            return configDto;
        }
        public async Task<bool> DeleteConfigAsync(int configId)
        {
            var config = (await _systemConfigRepository.GetAllConfigsAsync()).FirstOrDefault(c => c.ConfigId == configId);
            if (config == null) return false;
            await _systemConfigRepository.DeleteConfigAsync(configId);
            return true;
        }
    }
}