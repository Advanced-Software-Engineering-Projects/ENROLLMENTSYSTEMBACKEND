using ENROLLMENTSYSTEMBACKEND.Data;
using ENROLLMENTSYSTEMBACKEND.Models;
using Microsoft.EntityFrameworkCore;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly EnrollmentInformationDbContext _context;

        public CourseRepository(EnrollmentInformationDbContext context)
        {
            _context = context;
        }

        public async Task<Course?> GetCourseByIdAsync(string courseId)
        {
            return await _context.Courses
                .Include(c => c.Prerequisites)
                    .ThenInclude(p => p.PrerequisiteCourse)
                .FirstOrDefaultAsync(c => c.CourseId == courseId);
        }

        public async Task<Course?> GetCourseByCodeAsync(string courseCode)
        {
            return await _context.Courses
                .Include(c => c.Prerequisites)
                    .ThenInclude(p => p.PrerequisiteCourse)
                .FirstOrDefaultAsync(c => c.CourseCode == courseCode);
        }

        public async Task<List<Course>> GetAllCoursesAsync()
        {
            return await _context.Courses
                .Include(c => c.Prerequisites)
                    .ThenInclude(p => p.PrerequisiteCourse)
                .ToListAsync();
        }

        public async Task<List<Course>> GetCoursesByProgramAsync(string program)
        {
            return await _context.Courses
                .Include(c => c.Prerequisites)
                    .ThenInclude(p => p.PrerequisiteCourse)
                .Where(c => c.Program == program)
                .ToListAsync();
        }

        public async Task<List<Course>> GetCoursesByProgramAndYearAsync(string program, int year)
        {
            return await _context.Courses
                .Include(c => c.Prerequisites)
                    .ThenInclude(p => p.PrerequisiteCourse)
                .Where(c => c.Program == program && c.Year == year)
                .ToListAsync();
        }

        public async Task AddCourseAsync(Course course)
        {
            if (string.IsNullOrEmpty(course.CourseId))
            {
                course.CourseId = Guid.NewGuid().ToString();
            }
            
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCourseAsync(Course course)
        {
            var existing = await _context.Courses
                .Include(c => c.Prerequisites)
                .FirstOrDefaultAsync(c => c.CourseId == course.CourseId);

            if (existing != null)
            {
                existing.CourseCode = course.CourseCode;
                existing.CourseName = course.CourseName;
                existing.Credits = course.Credits;
                existing.Description = course.Description;
                existing.Program = course.Program;
                existing.Year = course.Year;
                existing.DueDate = course.DueDate;
                existing.IsActive = course.IsActive;

                // Update prerequisites
                _context.CoursePrerequisites.RemoveRange(existing.Prerequisites);
                existing.Prerequisites = course.Prerequisites;

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteCourseAsync(string courseId)
        {
            var course = await _context.Courses
                .Include(c => c.Prerequisites)
                .FirstOrDefaultAsync(c => c.CourseId == courseId);
                
            if (course != null)
            {
                _context.CoursePrerequisites.RemoveRange(course.Prerequisites);
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
            }
        }
    }
}