using Microsoft.EntityFrameworkCore;
using ENROLLMENTSYSTEMBACKEND.Data;
using ENROLLMENTSYSTEMBACKEND.IRepositories;
using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public class SystemConfigRepository : ISystemConfigRepository
    {
        private readonly FinancialAndAdminDbContext _context;

        public SystemConfigRepository(FinancialAndAdminDbContext context) => _context = context;

        public async Task<string> GetConfigValueAsync(string key) => (await _context.SystemConfigs.FirstOrDefaultAsync(c => c.Key == key))?.Value;
        public async Task<IEnumerable<SystemConfig>> GetAllConfigsAsync() => await _context.SystemConfigs.ToListAsync();
        public async Task AddConfigAsync(SystemConfig config) { await _context.SystemConfigs.AddAsync(config); await _context.SaveChangesAsync(); }
        public async Task UpdateConfigAsync(SystemConfig config) { _context.SystemConfigs.Update(config); await _context.SaveChangesAsync(); }
        public async Task DeleteConfigAsync(int configId)
        {
            var config = await _context.SystemConfigs.FindAsync(configId);
            if (config != null) { _context.SystemConfigs.Remove(config); await _context.SaveChangesAsync(); }
        }
    }
}