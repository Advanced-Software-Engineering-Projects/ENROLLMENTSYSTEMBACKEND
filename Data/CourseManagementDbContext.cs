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

}