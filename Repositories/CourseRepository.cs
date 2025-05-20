using Microsoft.EntityFrameworkCore;
using ENROLLMENTSYSTEMBACKEND.Data;
using ENROLLMENTSYSTEMBACKEND.IRepositories;
using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly CourseManagementDbContext _context;

        public CourseRepository(CourseManagementDbContext context) => _context = context;

        public async Task<Course> GetCourseByIdAsync(int courseId) => await _context.Courses.FindAsync(courseId);
        public async Task<IEnumerable<Course>> GetCoursesByIdsAsync(IEnumerable<int> courseIds) => await _context.Courses.Where(c => courseIds.Contains(c.CourseId)).ToListAsync();
        public async Task<IEnumerable<Course>> GetAllCoursesAsync() => await _context.Courses.ToListAsync();
        public async Task<int> GetTotalCreditsByCourseIdsAsync(IEnumerable<int> courseIds) => await _context.Courses.Where(c => courseIds.Contains(c.CourseId)).SumAsync(c => c.Credits);
        public async Task AddCourseAsync(Course course) { await _context.Courses.AddAsync(course); await _context.SaveChangesAsync(); }
    }
}