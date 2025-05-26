using ENROLLMENTSYSTEMBACKEND.Models;
using Microsoft.EntityFrameworkCore;

public class CourseManagementDbContext : DbContext
{
    public CourseManagementDbContext(DbContextOptions<CourseManagementDbContext> options)
        : base(options)
    {
    }

    public DbSet<Programs> Programs { get; set; }
    public DbSet<ProgramVersion> ProgramVersions { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<ProgramCourse> ProgramCourses { get; set; }
    public DbSet<Prerequisite> Prerequisites { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }

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