using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public interface ICourseRepository
    {
        Task<Course> GetCourseByIdAsync(string courseId);
        Task<List<Course>> GetCoursesAsync();
        Task<List<Course>> GetAllCoursesAsync();
        Task<Course> GetCourseByCodeAsync(string courseCode);
        Task AddCourseAsync(Course course);
        Task UpdateCourseAsync(Course course);
        Task DeleteCourseAsync(string courseCode);
    }
}