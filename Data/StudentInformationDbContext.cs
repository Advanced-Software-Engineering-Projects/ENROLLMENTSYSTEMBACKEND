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

}