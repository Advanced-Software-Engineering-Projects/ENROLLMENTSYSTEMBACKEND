using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.IRepositories
{
    public interface IPrerequisiteRepository
    {
        Task<IEnumerable<Prerequisite>> GetPrerequisitesForCourseAsync(int courseId);
        Task AddPrerequisiteAsync(Prerequisite prerequisite);
        Task<IEnumerable<Prerequisite>> GetPrerequisitesForCoursesAsync(IEnumerable<int> courseIds);
    }
}
