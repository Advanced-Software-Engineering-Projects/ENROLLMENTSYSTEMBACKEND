using Microsoft.EntityFrameworkCore;
using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Data
{
    public class FinancialAndAdminDbContext : DbContext
    {
        public FinancialAndAdminDbContext(DbContextOptions<FinancialAndAdminDbContext> options) : base(options) { }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Fee> Fees { get; set; }
        public DbSet<SystemConfig> SystemConfigs { get; set; }
        public DbSet<UserActivity> UserActivities { get; set; }

    }
}    