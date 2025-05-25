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
        // Map shared entities to unique tables and schemas
        modelBuilder.Entity<Course>().ToTable("Courses", schema: "CourseMgmt");
        modelBuilder.Entity<Enrollment>().ToTable("Enrollments", schema: "CourseMgmt");

        // Map other entities
        modelBuilder.Entity<Programs>().ToTable("Programs", schema: "CourseMgmt");
        modelBuilder.Entity<ProgramVersion>().ToTable("ProgramVersions", schema: "CourseMgmt");
        modelBuilder.Entity<ProgramCourse>().ToTable("ProgramCourses", schema: "CourseMgmt");
        modelBuilder.Entity<Prerequisite>().ToTable("Prerequisites", schema: "CourseMgmt");

        // Define primary keys
        modelBuilder.Entity<Course>().HasKey(c => c.CourseId);
        modelBuilder.Entity<Enrollment>().HasKey(e => e.EnrollmentId);
        modelBuilder.Entity<Programs>().HasKey(p => p.ProgramId);
        modelBuilder.Entity<ProgramVersion>().HasKey(pv => pv.ProgramVersionId);
        modelBuilder.Entity<ProgramCourse>().HasKey(pc => pc.ProgramCourseId);
        modelBuilder.Entity<Prerequisite>().HasKey(p => p.PrerequisiteId);

        // Configure relationships
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