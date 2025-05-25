namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public interface ISystemConfigService
    {
        Task<string> GetConfigValueAsync(string key);
        Task SetConfigValueAsync(string key, string value);
    }
}
