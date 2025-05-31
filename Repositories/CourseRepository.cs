using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly List<Course> _courses = new List<Course>();

        public async Task<List<Course>> GetAllCoursesAsync()
        {
            return await Task.FromResult(_courses);
        }

        public async Task<List<Course>> GetCoursesAsync()
        {
            return await Task.FromResult(_courses); 
        }

        public async Task<Course> GetCourseByCodeAsync(string courseCode)
        {
            return await Task.FromResult(_courses.FirstOrDefault(c => c.CourseCode == courseCode));
        }

        public async Task AddCourseAsync(Course course)
        {
            _courses.Add(course);
            await Task.CompletedTask;
        }

        public async Task UpdateCourseAsync(Course course)
        {
            var existingCourse = _courses.FirstOrDefault(c => c.CourseCode == course.CourseCode);
            if (existingCourse != null)
            {
                existingCourse.CourseName = course.CourseName;
                existingCourse.Description = course.Description;
                existingCourse.Prerequisites = course.Prerequisites;
            }
            await Task.CompletedTask;
        }

        public async Task<Course> GetCourseByIdAsync(string courseId)
        {
            return await Task.FromResult(_courses.FirstOrDefault(c => c.CourseId == courseId));
        }

        public async Task DeleteCourseAsync(string courseCode)
        {
            var course = _courses.FirstOrDefault(c => c.CourseCode == courseCode);
            if (course != null)
            {
                _courses.Remove(course);
            }
            await Task.CompletedTask;
        }
    }
}