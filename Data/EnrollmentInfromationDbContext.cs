using ENROLLMENTSYSTEMBACKEND.Models;
using Microsoft.EntityFrameworkCore;

namespace ENROLLMENTSYSTEMBACKEND.Data
{
    public class EnrollmentInfromation : DbContext
    {
        public EnrollmentInfromation(DbContextOptions<EnrollmentInfromation> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Programs> Programs { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Fee> Fees { get; set; }
        public DbSet<FeeHold> FeeHolds { get; set; }
        public DbSet<Hold> Holds { get; set; }
        public DbSet<FormSubmission> FormSubmissions { get; set; }
        public DbSet<PaymentRecord> PaymentRecords { get; set; }
        public DbSet<PendingRequest> PendingRequests { get; set; }
        public DbSet<Prerequisite> Prerequisites { get; set; }
        public DbSet<Timetable> Timetables { get; set; }
        public DbSet<RegistrationPeriod> RegistrationPeriods { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Grade>()
                .HasKey(g => new { g.StudentId, g.CourseId });
            modelBuilder.Entity<Prerequisite>()
                .HasKey(p => new { p.CourseCode, p.PrerequisiteCourseCode });



        }

    }
}
