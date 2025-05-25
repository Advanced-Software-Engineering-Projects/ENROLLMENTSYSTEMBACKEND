using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public interface ICourseRepository
    {
        Task<List<Course>> GetAllAsync();
        Task<List<Course>> GetAvailableCoursesAsync(string semester);
        Task<List<Course>> GetCoursesBySemesterAsync(string semester);
        Task<List<Prerequisite>> GetPrerequisitesAsync(int courseId);
        Task<List<Prerequisite>> GetAllPrerequisitesAsync();
    }
}