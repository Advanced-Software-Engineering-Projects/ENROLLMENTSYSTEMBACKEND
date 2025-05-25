using ENROLLMENTSYSTEMBACKEND.Models;
using Microsoft.EntityFrameworkCore;

public class StudentInformationDbContext : DbContext
{
    public StudentInformationDbContext(DbContextOptions<StudentInformationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Student> Students { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<FormSubmission> FormSubmissions { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Prerequisite> Prerequisites { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Map entities to specific tables and schemas
        modelBuilder.Entity<Student>().ToTable("Students", schema: "StudentInfo");
        modelBuilder.Entity<Course>().ToTable("Courses", schema: "StudentInfo");
        modelBuilder.Entity<Enrollment>().ToTable("Enrollments", schema: "StudentInfo");
        modelBuilder.Entity<FormSubmission>().ToTable("FormSubmissions", schema: "StudentInfo");
        modelBuilder.Entity<Prerequisite>().ToTable("Prerequisites", schema: "StudentInfo");

        // Define primary keys
        modelBuilder.Entity<Student>().HasKey(s => s.StudentId);
        modelBuilder.Entity<Course>().HasKey(c => c.CourseId);
        modelBuilder.Entity<Enrollment>().HasKey(e => e.EnrollmentId);
        modelBuilder.Entity<FormSubmission>().HasKey(fs => fs.Id);
        modelBuilder.Entity<Prerequisite>().HasKey(p => p.PrerequisiteId);

        // Configure relationships
        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Student)
            .WithMany(s => s.Enrollments)
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Course)
            .WithMany(c => c.Enrollments)
            .HasForeignKey(e => e.CourseId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Prerequisite>()
            .HasOne(p => p.Course)
            .WithMany(c => c.Prerequisites)
            .HasForeignKey(p => p.CourseId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}