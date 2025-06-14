using Microsoft.EntityFrameworkCore;
using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Data
{
    public class EnrollmentInformation : DbContext
    {
        public EnrollmentInformation(DbContextOptions<EnrollmentInformation> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<FormSubmission> FormSubmissions { get; set; }
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
    }
}