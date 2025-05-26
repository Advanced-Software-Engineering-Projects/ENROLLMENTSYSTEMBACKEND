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
        base.OnModelCreating(modelBuilder);

        // Configure the Prerequisite entity
        modelBuilder.Entity<Prerequisite>()
            .HasOne(p => p.Course) // The Course that requires the prerequisite
            .WithMany(c => c.Prerequisites) // The Course has many Prerequisites
            .HasForeignKey(p => p.CourseId) // Foreign key for Course
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes

        modelBuilder.Entity<Prerequisite>()
            .HasOne(p => p.PrerequisiteCourse) // The Course that is the prerequisite
            .WithMany() // No inverse navigation property
            .HasForeignKey(p => p.PrerequisiteCourseId) // Foreign key for PrerequisiteCourse
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes
    }
}