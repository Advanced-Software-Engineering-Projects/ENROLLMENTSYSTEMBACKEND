using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public interface ICourseRepository
    {
        Task<Course?> GetCourseByIdAsync(string courseId);
        Task<Course?> GetCourseByCodeAsync(string courseCode);
        Task<List<Course>> GetAllCoursesAsync();
        Task<List<Course>> GetCoursesByProgramAsync(string program);
        Task<List<Course>> GetCoursesByProgramAndYearAsync(string program, int year);
        Task AddCourseAsync(Course course);
        Task UpdateCourseAsync(Course course);
        Task DeleteCourseAsync(string courseId);
    }
}