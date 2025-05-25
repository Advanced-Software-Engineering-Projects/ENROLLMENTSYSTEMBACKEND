using Microsoft.EntityFrameworkCore;
using ENROLLMENTSYSTEMBACKEND.Models;
using System.Threading.Tasks;
using ENROLLMENTSYSTEMBACKEND.Data;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public class SystemConfigRepository : ISystemConfigRepository
    {
        private readonly FinancialAndAdminDbContext _context;

        public SystemConfigRepository(FinancialAndAdminDbContext context)
        {
            _context = context;
        }

        public async Task<SystemConfig> GetConfigByKeyAsync(string key)
        {
            return await _context.SystemConfigs.FirstOrDefaultAsync(c => c.Key == key);
        }

        public async Task AddConfigAsync(SystemConfig config)
        {
            await _context.SystemConfigs.AddAsync(config);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateConfigAsync(SystemConfig config)
        {
            _context.SystemConfigs.Update(config);
            await _context.SaveChangesAsync();
        }
    }
}