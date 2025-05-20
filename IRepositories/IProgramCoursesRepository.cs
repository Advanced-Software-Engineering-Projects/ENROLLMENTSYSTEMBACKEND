using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.IRepositories
{
    public interface IProgramCoursesRepository
    {
        Task<IEnumerable<ProgramCourses>> GetProgramCoursesByProgramVersionAsync(int programVersionId);
        Task AddProgramCourseAsync(ProgramCourses programCourse);
        Task DeleteProgramCourseAsync(int programCourseId);
    }
}
