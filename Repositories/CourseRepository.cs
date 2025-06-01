using ENROLLMENTSYSTEMBACKEND.Models;
using ENROLLMENTSYSTEMBACKEND.Data;
using Microsoft.EntityFrameworkCore;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly EnrollmentInfromation _context;

        public CourseRepository(EnrollmentInfromation context)
        {
            _context = context;
        }

        public async Task<List<Course>> GetAllCoursesAsync()
        {
            return await _context.Courses.ToListAsync();
        }

        public async Task<List<Course>> GetCoursesAsync()
        {
            return await _context.Courses.ToListAsync();
        }

        public async Task<Course> GetCourseByCodeAsync(string courseCode)
        {
            return await _context.Courses.FirstOrDefaultAsync(c => c.CourseCode == courseCode);
        }

        public async Task AddCourseAsync(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCourseAsync(Course course)
        {
            var existingCourse = await _context.Courses.FirstOrDefaultAsync(c => c.CourseCode == course.CourseCode);
            if (existingCourse != null)
            {
                existingCourse.CourseName = course.CourseName;
                existingCourse.Description = course.Description;
                existingCourse.Prerequisites = course.Prerequisites;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Course> GetCourseByIdAsync(string courseId)
        {
            return await _context.Courses.FirstOrDefaultAsync(c => c.CourseId == courseId);
        }

        public async Task DeleteCourseAsync(string courseCode)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.CourseCode == courseCode);
            if (course != null)
            {
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
            }
        }
    }
}
