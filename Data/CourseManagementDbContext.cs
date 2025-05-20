using Microsoft.EntityFrameworkCore;
using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Data
{
    public class CourseManagementDbContext : DbContext
    {
        public CourseManagementDbContext(DbContextOptions<CourseManagementDbContext> options) : base(options) { }

        public DbSet<Programs> Programs { get; set; }
        public DbSet<ProgramVersion> ProgramVersions { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<ProgramCourses> ProgramCourses { get; set; }
        public DbSet<Prerequisite> Prerequisites { get; set; }

    }
}