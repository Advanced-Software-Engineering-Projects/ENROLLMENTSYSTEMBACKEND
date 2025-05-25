using Microsoft.EntityFrameworkCore;
using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly CourseManagementDbContext _context;

        public CourseRepository(CourseManagementDbContext context)
        {
            _context = context;
        }

        public async Task<List<Course>> GetAllAsync()
        {
            return await _context.Courses.ToListAsync();
        }

        public async Task<List<Course>> GetCoursesBySemesterAsync(string semester)
        {
            return await _context.Courses
                .Where(c => c.SemesterOffered == semester)
                .ToListAsync();
        }

        public async Task<List<Prerequisite>> GetPrerequisitesAsync(int courseId)
        {
            return await _context.Prerequisites
                .Where(p => p.CourseId == courseId)
                .Include(p => p.PrerequisiteCourse)
                .ToListAsync();
        }

        public async Task<List<Prerequisite>> GetAllPrerequisitesAsync()
        {
            return await _context.Prerequisites
                .Include(p => p.Course)
                .Include(p => p.PrerequisiteCourse)
                .ToListAsync();
        }

        public async Task<List<Course>> GetAvailableCoursesAsync(string semester)
        {
            return await _context.Courses
                .Where(c => c.SemesterOffered == semester && c.IsAvailable)
                .ToListAsync();
        }
    }
}