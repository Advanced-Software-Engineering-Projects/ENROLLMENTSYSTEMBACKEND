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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Map shared entities to unique tables and schemas
            modelBuilder.Entity<Student>().ToTable("Students", schema: "FinancialAndAdmin");
            modelBuilder.Entity<Course>().ToTable("Courses", schema: "FinancialAndAdmin");

            // Map other entities
            modelBuilder.Entity<Admin>().ToTable("Admins", schema: "FinancialAndAdmin");
            modelBuilder.Entity<Fee>().ToTable("Fees", schema: "FinancialAndAdmin");
            modelBuilder.Entity<PendingRequest>().ToTable("PendingRequests", schema: "FinancialAndAdmin");
            modelBuilder.Entity<EnrollmentData>().ToTable("EnrollmentData", schema: "FinancialAndAdmin");
            modelBuilder.Entity<CompletionRate>().ToTable("CompletionRates", schema: "FinancialAndAdmin");
            modelBuilder.Entity<Hold>().ToTable("Holds", schema: "FinancialAndAdmin");
            modelBuilder.Entity<RegistrationPeriod>().ToTable("RegistrationPeriods", schema: "FinancialAndAdmin");
            modelBuilder.Entity<SystemConfig>().ToTable("SystemConfigs", schema: "FinancialAndAdmin");
            modelBuilder.Entity<UserActivity>().ToTable("UserActivities", schema: "FinancialAndAdmin");

            // Define primary keys
            modelBuilder.Entity<Student>().HasKey(s => s.StudentId);
            modelBuilder.Entity<Course>().HasKey(c => c.CourseId);
            modelBuilder.Entity<Admin>().HasKey(a => a.AdminId);
            modelBuilder.Entity<Fee>().HasKey(f => f.FeeId);
            modelBuilder.Entity<PendingRequest>().HasKey(pr => pr.RequestId);
            modelBuilder.Entity<EnrollmentData>().HasKey(ed => ed.EnrollmentDataId);
            modelBuilder.Entity<CompletionRate>().HasKey(cr => cr.CompletionRateId);
            modelBuilder.Entity<Hold>().HasKey(h => h.HoldId);
            modelBuilder.Entity<RegistrationPeriod>().HasKey(rp => rp.RegistrationPeriodId);
            modelBuilder.Entity<SystemConfig>().HasKey(sc => sc.ConfigId);
            modelBuilder.Entity<UserActivity>().HasKey(ua => ua.ActivityId);

            // Optional: Ignore navigation properties not needed in this context
            modelBuilder.Entity<Student>().Ignore(s => s.Enrollments);
        }
    }
}