using Microsoft.EntityFrameworkCore;
using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Data
{
    public class EnrollmentInformationDbContext : DbContext
    {
        public EnrollmentInformationDbContext(DbContextOptions<EnrollmentInformationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<FormSubmission> FormSubmissions { get; set; }
        public DbSet<ReconsiderationForm> ReconsiderationForms { get; set; }
        public DbSet<CompassionateAegrotatForm> CompassionateAegrotatForms { get; set; }
        public DbSet<CompletionProgrammeForm> CompletionProgrammeForms { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<ServiceHold> ServiceHolds { get; set; }
        public DbSet<Fee> Fees { get; set; }
        public DbSet<FeeHold> FeeHolds { get; set; }
        public DbSet<PaymentRecord> PaymentRecords { get; set; }
        public DbSet<Timetable> Timetables { get; set; }
        public DbSet<RegistrationPeriod> RegistrationPeriods { get; set; }
        public DbSet<GradeRecheckRequest> GradeRecheckRequests { get; set; }
        public DbSet<GradeNotification> GradeNotifications { get; set; }
        public DbSet<Prerequisite> Prerequisite { get; set; }
        public DbSet<Programs> Programs { get; set; }
        public DbSet<UserLog> UserLogs { get; set; }
        public DbSet<PendingRequest> PendingRequests { get; set; }
        public DbSet<Hold> Holds { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configure FormSubmission inheritance
            modelBuilder.Entity<FormSubmission>()
                .HasDiscriminator<string>("FormType")
                .HasValue<ReconsiderationForm>("Reconsideration")
                .HasValue<CompassionateAegrotatForm>("CompassionateAegrotat")
                .HasValue<CompletionProgrammeForm>("CompletionProgramme");

            // Configure relationships and constraints
            modelBuilder.Entity<Grade>()
                .HasKey(g => new { g.StudentId, g.CourseId });

            modelBuilder.Entity<Enrollment>()
                .HasKey(e => new { e.StudentId, e.CourseId });

            modelBuilder.Entity<PaymentRecord>()
                .HasKey(p => new { p.StudentId, p.FeeId });

            modelBuilder.Entity<Prerequisite>()
                .HasKey(p => new { p.CourseCode, p.PrerequisiteCourseCode });

            modelBuilder.Entity<ServiceHold>()
                .HasKey(s => new { s.StudentId, s.Service });

            modelBuilder.Entity<Timetable>()
                .HasKey(t => new { t.StudentId, t.CourseCode, t.Semester });
        }
    }
}