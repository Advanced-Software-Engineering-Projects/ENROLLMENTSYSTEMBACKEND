using ENROLLMENTSYSTEMBACKEND.Models;
using Microsoft.EntityFrameworkCore;

namespace ENROLLMENTSYSTEMBACKEND.Data 
{
    public class FinancialAndAdminDbContext : DbContext
    {
        public FinancialAndAdminDbContext(DbContextOptions<FinancialAndAdminDbContext> options)
            : base(options)
        {
        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Fee> Fees { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<PendingRequest> PendingRequests { get; set; }
        public DbSet<EnrollmentData> EnrollmentData { get; set; }
        public DbSet<CompletionRate> CompletionRates { get; set; }
        public DbSet<Hold> Holds { get; set; }
        public DbSet<RegistrationPeriod> RegistrationPeriods { get; set; }
        public DbSet<SystemConfig> SystemConfigs { get; set; }
        public DbSet<UserActivity> UserActivities { get; set; }

    }
}