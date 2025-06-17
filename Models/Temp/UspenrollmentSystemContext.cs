using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ENROLLMENTSYSTEMBACKEND.Models.Temp;

public partial class UspenrollmentSystemContext : DbContext
{
    public UspenrollmentSystemContext()
    {
    }

    public UspenrollmentSystemContext(DbContextOptions<UspenrollmentSystemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ClosedRegistration> ClosedRegistrations { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<CoursePrerequisite> CoursePrerequisites { get; set; }

    public virtual DbSet<CourseRegistrationPeriod> CourseRegistrationPeriods { get; set; }

    public virtual DbSet<Enrollment> Enrollments { get; set; }

    public virtual DbSet<Fee> Fees { get; set; }

    public virtual DbSet<FeeHold> FeeHolds { get; set; }

    public virtual DbSet<FormSubmission> FormSubmissions { get; set; }

    public virtual DbSet<Grade> Grades { get; set; }

    public virtual DbSet<GradeNotification> GradeNotifications { get; set; }

    public virtual DbSet<GradeRecheckRequest> GradeRecheckRequests { get; set; }

    public virtual DbSet<Hold> Holds { get; set; }

    public virtual DbSet<PaymentRecord> PaymentRecords { get; set; }

    public virtual DbSet<PendingRequest> PendingRequests { get; set; }

    public virtual DbSet<Prerequisite> Prerequisites { get; set; }

    public virtual DbSet<Program> Programs { get; set; }

    public virtual DbSet<RegistrationPeriod> RegistrationPeriods { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<ServiceHold> ServiceHolds { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Timetable> Timetables { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserLog> UserLogs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CoursePrerequisite>(entity =>
        {
            entity.HasIndex(e => e.CourseId, "IX_CoursePrerequisites_CourseId");

            entity.HasIndex(e => e.PrerequisiteCourseId, "IX_CoursePrerequisites_PrerequisiteCourseId");

            entity.HasOne(d => d.Course).WithMany(p => p.CoursePrerequisiteCourses).HasForeignKey(d => d.CourseId);

            entity.HasOne(d => d.PrerequisiteCourse).WithMany(p => p.CoursePrerequisitePrerequisiteCourses)
                .HasForeignKey(d => d.PrerequisiteCourseId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.HasKey(e => new { e.StudentId, e.CourseId });

            entity.HasIndex(e => e.CourseId, "IX_Enrollments_CourseId");

            entity.HasOne(d => d.Course).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Student).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Fee>(entity =>
        {
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<FormSubmission>(entity =>
        {
            entity.HasKey(e => e.SubmissionId);

            entity.HasIndex(e => e.CourseId, "IX_FormSubmissions_CourseId");

            entity.HasIndex(e => e.ProgramId, "IX_FormSubmissions_ProgramId");

            entity.HasOne(d => d.Course).WithMany(p => p.FormSubmissions).HasForeignKey(d => d.CourseId);

            entity.HasOne(d => d.Program).WithMany(p => p.FormSubmissions).HasForeignKey(d => d.ProgramId);
        });

        modelBuilder.Entity<Grade>(entity =>
        {
            entity.HasKey(e => new { e.StudentId, e.CourseId });

            entity.HasIndex(e => e.CourseId, "IX_Grades_CourseId");

            entity.HasOne(d => d.Course).WithMany(p => p.Grades)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Student).WithMany(p => p.Grades)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<GradeNotification>(entity =>
        {
            entity.HasIndex(e => e.CourseId, "IX_GradeNotifications_CourseId");

            entity.HasIndex(e => e.StudentId, "IX_GradeNotifications_StudentId");

            entity.HasOne(d => d.Course).WithMany(p => p.GradeNotifications)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Student).WithMany(p => p.GradeNotifications)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<GradeRecheckRequest>(entity =>
        {
            entity.HasIndex(e => e.CourseId, "IX_GradeRecheckRequests_CourseId");

            entity.HasIndex(e => new { e.StudentId, e.CourseId }, "IX_GradeRecheckRequests_StudentId_CourseId");

            entity.HasOne(d => d.Course).WithMany(p => p.GradeRecheckRequests)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Student).WithMany(p => p.GradeRecheckRequests)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Grade).WithMany(p => p.GradeRecheckRequests)
                .HasForeignKey(d => new { d.StudentId, d.CourseId })
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Hold>(entity =>
        {
            entity.HasIndex(e => e.StudentId, "IX_Holds_StudentId");

            entity.HasOne(d => d.Student).WithMany(p => p.Holds)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<PaymentRecord>(entity =>
        {
            entity.HasKey(e => new { e.StudentId, e.FeeId });

            entity.Property(e => e.AmountPaid).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<PendingRequest>(entity =>
        {
            entity.HasIndex(e => e.CourseCode, "IX_PendingRequests_CourseCode");

            entity.HasIndex(e => e.StudentId, "IX_PendingRequests_StudentId");

            entity.HasOne(d => d.CourseCodeNavigation).WithMany(p => p.PendingRequests)
                .HasForeignKey(d => d.CourseCode)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Student).WithMany(p => p.PendingRequests)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Prerequisite>(entity =>
        {
            entity.HasKey(e => new { e.CourseCode, e.PrerequisiteCourseCode });
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.ServicesId);
        });

        modelBuilder.Entity<ServiceHold>(entity =>
        {
            entity.HasKey(e => new { e.StudentId, e.ServiceId });

            entity.HasIndex(e => e.ServiceId, "IX_ServiceHolds_ServiceId");

            entity.HasOne(d => d.Service).WithMany(p => p.ServiceHolds)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Student).WithMany(p => p.ServiceHolds)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Timetable>(entity =>
        {
            entity.HasKey(e => new { e.StudentId, e.CourseCode, e.Semester });
        });

        modelBuilder.Entity<UserLog>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_UserLogs_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.UserLogs).HasForeignKey(d => d.UserId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
