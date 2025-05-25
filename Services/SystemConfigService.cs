using ENROLLMENTSYSTEMBACKEND.Models;
using ENROLLMENTSYSTEMBACKEND.Repositories;
using System.Threading.Tasks;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public class SystemConfigService : ISystemConfigService
    {
        private readonly ISystemConfigRepository _repository;

        public SystemConfigService(ISystemConfigRepository repository)
        {
            _repository = repository;
        }

        public async Task<string> GetConfigValueAsync(string key)
        {
            var config = await _repository.GetConfigByKeyAsync(key);
            return config?.Value;
        }

        public async Task SetConfigValueAsync(string key, string value)
        {
            var config = await _repository.GetConfigByKeyAsync(key);
            if (config == null)
            {
                config = new SystemConfig { Key = key, Value = value };
                await _repository.AddConfigAsync(config);
            }
            else
            {
                config.Value = value;
                await _repository.UpdateConfigAsync(config);
            }
        }
    }
}