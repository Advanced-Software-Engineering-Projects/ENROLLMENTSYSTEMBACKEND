using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.IRepositories
{
    public interface ICourseRepository
    {
        Task<Course> GetCourseByIdAsync(int courseId);
        Task<IEnumerable<Course>> GetCoursesByIdsAsync(IEnumerable<int> courseIds);
        Task<IEnumerable<Course>> GetAllCoursesAsync();
        Task<int> GetTotalCreditsByCourseIdsAsync(IEnumerable<int> courseIds);
        Task AddCourseAsync(Course course);
    }
}
