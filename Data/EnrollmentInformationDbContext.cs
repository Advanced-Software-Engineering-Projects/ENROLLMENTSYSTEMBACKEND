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

        // DbSets for all 25 models
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<FormSubmission> FormSubmissions { get; set; }
        public DbSet<CompassionateAegrotatForm> CompassionateAegrotatForms { get; set; } // Added for TPH
        public DbSet<CompletionProgrammeForm> CompletionProgrammeForms { get; set; } // Added for TPH
        public DbSet<ReconsiderationForm> ReconsiderationForms { get; set; } // Added for TPH
        public DbSet<GradeRecheckRequest> GradeRecheckRequests { get; set; }
        public DbSet<GradeNotification> GradeNotifications { get; set; }
        public DbSet<ServiceHold> ServiceHolds { get; set; }
        public DbSet<Fee> Fees { get; set; }
        public DbSet<PaymentRecord> PaymentRecords { get; set; }
        public DbSet<Programs> Programs { get; set; }
        public DbSet<FeeHold> FeeHolds { get; set; }
        public DbSet<Prerequisite> Prerequisites { get; set; }
        public DbSet<RegistrationPeriod> RegistrationPeriods { get; set; }
        public DbSet<Timetable> Timetables { get; set; }
        public DbSet<UserLog> UserLogs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ClosedRegistration> ClosedRegistrations { get; set; }
        public DbSet<CourseRegistrationPeriod> CourseRegistrationPeriods { get; set; }
        public DbSet<Hold> Holds { get; set; }
        public DbSet<PendingRequest> PendingRequests { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<CoursePrerequisite> CoursePrerequisites { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure FormSubmission with TPH
            modelBuilder.Entity<FormSubmission>()
                .ToTable("FormSubmissions")
                .HasDiscriminator<string>("FormType")
                .HasValue<FormSubmission>("FormSubmission")
                .HasValue<CompassionateAegrotatForm>("CompassionateAegrotat")
                .HasValue<CompletionProgrammeForm>("CompletionProgramme")
                .HasValue<ReconsiderationForm>("Reconsideration");

            // Configure properties for FormSubmission
            modelBuilder.Entity<FormSubmission>()
                .Property(f => f.SubmissionId)
                .HasColumnName("SubmissionId")
                .IsRequired();
            modelBuilder.Entity<FormSubmission>()
                .Property(f => f.StudentId)
                .HasColumnName("StudentId")
                .IsRequired();
            modelBuilder.Entity<FormSubmission>()
                .Property(f => f.FormType)
                .HasColumnName("FormType")
                .IsRequired();
            modelBuilder.Entity<FormSubmission>()
                .Property(f => f.FullName)
                .HasColumnName("FullName");
            modelBuilder.Entity<FormSubmission>()
                .Property(f => f.Email)
                .HasColumnName("Email");
            modelBuilder.Entity<FormSubmission>()
                .Property(f => f.Telephone)
                .HasColumnName("Telephone");
            modelBuilder.Entity<FormSubmission>()
                .Property(f => f.PostalAddress)
                .HasColumnName("PostalAddress");
            modelBuilder.Entity<FormSubmission>()
                .Property(f => f.Status)
                .HasColumnName("Status");
            modelBuilder.Entity<FormSubmission>()
                .Property(f => f.EmailStatus)
                .HasColumnName("EmailStatus");
            modelBuilder.Entity<FormSubmission>()
                .Property(f => f.SubmissionDate)
                .HasColumnName("SubmissionDate");
            modelBuilder.Entity<FormSubmission>()
                .Property(f => f.FormData)
                .HasColumnName("FormData");
            modelBuilder.Entity<FormSubmission>()
                .Property(f => f.SupportingDocuments)
                .HasColumnName("SupportingDocuments");
            modelBuilder.Entity<FormSubmission>()
                .Property(f => f.DateOfBirth)
                .HasColumnName("DateOfBirth");
            modelBuilder.Entity<FormSubmission>()
                .Property(f => f.Sponsorship)
                .HasColumnName("Sponsorship");
            modelBuilder.Entity<FormSubmission>()
                .Property(f => f.CourseCode)
                .HasColumnName("CourseCode");
            modelBuilder.Entity<FormSubmission>()
                .Property(f => f.CourseLecturer)
                .HasColumnName("CourseLecturer");
            modelBuilder.Entity<FormSubmission>()
                .Property(f => f.CourseTitle)
                .HasColumnName("CourseTitle");
            modelBuilder.Entity<FormSubmission>()
                .Property(f => f.ReceiptNo)
                .HasColumnName("ReceiptNo");
            modelBuilder.Entity<FormSubmission>()
                .Property(f => f.PaymentConfirmation)
                .HasColumnName("PaymentConfirmation");
            modelBuilder.Entity<FormSubmission>()
                .Property(f => f.CurrentGrade)
                .HasColumnName("CurrentGrade");
            modelBuilder.Entity<FormSubmission>()
                .Property(f => f.Campus)
                .HasColumnName("Campus");
            modelBuilder.Entity<FormSubmission>()
                .Property(f => f.Semester)
                .HasColumnName("Semester");
            modelBuilder.Entity<FormSubmission>()
                .Property(f => f.Year)
                .HasColumnName("Year");
            modelBuilder.Entity<FormSubmission>()
                .Property(f => f.Reason)
                .HasColumnName("Reason");
            modelBuilder.Entity<FormSubmission>()
                .Property(f => f.ApplicantSignature)
                .HasColumnName("ApplicantSignature");
            modelBuilder.Entity<FormSubmission>()
                .Property(f => f.Date)
                .HasColumnName("Date");
            modelBuilder.Entity<FormSubmission>()
                .Property(f => f.Programme)
                .HasColumnName("Programme");
            modelBuilder.Entity<FormSubmission>()
                .Property(f => f.ExamDate)
                .HasColumnName("ExamDate");
            modelBuilder.Entity<FormSubmission>()
                .Property(f => f.ExamStartTime)
                .HasColumnName("ExamStartTime");
            modelBuilder.Entity<FormSubmission>()
                .Property(f => f.ApplyingFor)
                .HasColumnName("ApplyingFor");
            modelBuilder.Entity<FormSubmission>()
                .Property(f => f.NewGrade)
                .HasColumnName("NewGrade");
            modelBuilder.Entity<FormSubmission>()
                .Property(f => f.MissedExams)
                .HasColumnName("MissedExams");
            modelBuilder.Entity<FormSubmission>()
                .Property(f => f.DeclarationAgreed)
                .HasColumnName("DeclarationAgreed");

            // Configure properties for derived types
            modelBuilder.Entity<CompassionateAegrotatForm>()
                .Property(f => f.MedicalDocumentation)
                .HasColumnName("MedicalDocumentation");
            modelBuilder.Entity<CompassionateAegrotatForm>()
                .Property(f => f.Comments)
                .HasColumnName("Comments");
            modelBuilder.Entity<CompassionateAegrotatForm>()
                .Property(f => f.CourseId)
                .HasColumnName("CourseId");
            modelBuilder.Entity<CompletionProgrammeForm>()
                .Property(f => f.Comments)
                .HasColumnName("Comments");
            modelBuilder.Entity<CompletionProgrammeForm>()
                .Property(f => f.IsCompleted)
                .HasColumnName("IsCompleted");
            modelBuilder.Entity<CompletionProgrammeForm>()
                .Property(f => f.ProgramCode)
                .HasColumnName("ProgramCode");
            modelBuilder.Entity<CompletionProgrammeForm>()
                .Property(f => f.ProgramId)
                .HasColumnName("ProgramId");
            modelBuilder.Entity<ReconsiderationForm>()
                .Property(f => f.Comments)
                .HasColumnName("Comments");
            modelBuilder.Entity<ReconsiderationForm>()
                .Property(f => f.CourseId)
                .HasColumnName("CourseId");

            // Configure foreign keys for derived types
            modelBuilder.Entity<CompassionateAegrotatForm>()
                .HasOne(f => f.Course)
                .WithMany()
                .HasForeignKey(f => f.CourseId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<CompletionProgrammeForm>()
                .HasOne(f => f.Program)
                .WithMany()
                .HasForeignKey(f => f.ProgramId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ReconsiderationForm>()
                .HasOne(f => f.Course)
                .WithMany()
                .HasForeignKey(f => f.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure GradeRecheckRequest
            modelBuilder.Entity<GradeRecheckRequest>()
                .ToTable("GradeRecheckRequests")
                .HasKey(g => g.Id);

            modelBuilder.Entity<GradeRecheckRequest>()
                .HasOne(g => g.Student)
                .WithMany()
                .HasForeignKey(g => g.StudentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<GradeRecheckRequest>()
                .HasOne(g => g.Course)
                .WithMany()
                .HasForeignKey(g => g.CourseId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<GradeRecheckRequest>()
                .HasOne(g => g.Grade)
                .WithMany()
                .HasForeignKey(g => new { g.StudentId, g.CourseId })
                .OnDelete(DeleteBehavior.NoAction);

            // Configure other entities
            modelBuilder.Entity<Grade>()
                .ToTable("Grades")
                .HasKey(g => new { g.StudentId, g.CourseId });

            modelBuilder.Entity<Enrollment>()
                .ToTable("Enrollments")
                .HasKey(e => new { e.StudentId, e.CourseId });

            modelBuilder.Entity<PaymentRecord>()
                .ToTable("PaymentRecords")
                .HasKey(p => new { p.StudentId, p.FeeId });

            modelBuilder.Entity<PaymentRecord>()
                .Property(p => p.AmountPaid)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Fee>()
                .ToTable("Fees")
                .Property(f => f.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Prerequisite>()
                .ToTable("Prerequisites")
                .HasKey(p => new { p.CourseCode, p.PrerequisiteCourseCode });

            modelBuilder.Entity<ServiceHold>()
                .ToTable("ServiceHolds")
                .HasKey(s => new { s.StudentId, s.ServiceId });

            modelBuilder.Entity<Timetable>()
                .ToTable("Timetables")
                .HasKey(t => new { t.StudentId, t.CourseCode, t.Semester });

            modelBuilder.Entity<Student>()
                .ToTable("Students")
                .HasKey(s => s.Id);

            modelBuilder.Entity<Course>()
                .ToTable("Courses")
                .HasKey(c => c.CourseId);

            modelBuilder.Entity<GradeNotification>()
                .ToTable("GradeNotifications")
                .HasKey(g => g.Id);

            modelBuilder.Entity<Programs>()
                .ToTable("Programs")
                .HasKey(p => p.Id);

            modelBuilder.Entity<FeeHold>()
                .ToTable("FeeHolds")
                .HasKey(f => f.Id);

            modelBuilder.Entity<RegistrationPeriod>()
                .ToTable("RegistrationPeriods")
                .HasKey(r => r.RegistrationPeriodId);

            modelBuilder.Entity<UserLog>()
                .ToTable("UserLogs")
                .HasKey(u => u.UserLogId);

            modelBuilder.Entity<User>()
                .ToTable("Users")
                .HasKey(u => u.Id);

            // Configure new entities
            modelBuilder.Entity<ClosedRegistration>()
                .ToTable("ClosedRegistrations")
                .HasKey(c => c.Id);

            modelBuilder.Entity<CourseRegistrationPeriod>()
                .ToTable("CourseRegistrationPeriods")
                .HasKey(c => c.Id);

            modelBuilder.Entity<Hold>()
                .ToTable("Holds")
                .HasKey(h => h.Id);

            modelBuilder.Entity<Hold>()
                .HasOne<Student>()
                .WithMany()
                .HasForeignKey(h => h.StudentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<PendingRequest>()
                .ToTable("PendingRequests")
                .HasKey(p => p.Id);

            modelBuilder.Entity<PendingRequest>()
                .HasOne<Student>()
                .WithMany()
                .HasForeignKey(p => p.StudentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Service>()
                .ToTable("Services")
                .HasKey(s => s.ServicesId);

            // Configure navigation properties for existing entities
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany()
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Student)
                .WithMany()
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Grade>()
                .HasOne(g => g.Student)
                .WithMany()
                .HasForeignKey(g => g.StudentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Grade>()
                .HasOne(g => g.Course)
                .WithMany()
                .HasForeignKey(g => g.CourseId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<GradeNotification>()
                .HasOne(g => g.Student)
                .WithMany()
                .HasForeignKey(g => g.StudentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<GradeNotification>()
                .HasOne(g => g.Course)
                .WithMany()
                .HasForeignKey(g => g.CourseId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ServiceHold>()
                .HasOne(s => s.Student)
                .WithMany()
                .HasForeignKey(s => s.StudentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ServiceHold>()
                .HasOne(s => s.Service)
                .WithMany()
                .HasForeignKey(s => s.ServiceId)
                .OnDelete(DeleteBehavior.NoAction);

            // Optional: Configure PendingRequest.CourseCode relationship
            modelBuilder.Entity<PendingRequest>()
                .HasOne<Course>()
                .WithMany()
                .HasForeignKey(p => p.CourseCode)
                .OnDelete(DeleteBehavior.NoAction);

            // Configure Course and CoursePrerequisite relationships
            modelBuilder.Entity<Course>()
                .HasMany(c => c.Prerequisites)
                .WithOne(p => p.Course)
                .HasForeignKey(p => p.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CoursePrerequisite>()
                .HasOne(p => p.PrerequisiteCourse)
                .WithMany()
                .HasForeignKey(p => p.PrerequisiteCourseId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}