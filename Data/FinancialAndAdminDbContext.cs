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
            base.OnModelCreating(modelBuilder);

            // Configure Course-Prerequisite relationships
            modelBuilder.Entity<Prerequisite>()
                .HasOne(p => p.Course)
                .WithMany(c => c.Prerequisites)
                .HasForeignKey(p => p.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Prerequisite>()
                .HasOne(p => p.PrerequisiteCourse)
                .WithMany()
                .HasForeignKey(p => p.PrerequisiteCourseId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}