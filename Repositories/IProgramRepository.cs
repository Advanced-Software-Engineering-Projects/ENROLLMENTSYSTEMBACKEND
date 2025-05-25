using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public interface IProgramRepository
    {
        Task<Programs> GetByIdAsync(int id);
        Task<List<ProgramCourse>> GetProgramCoursesAsync(int programVersionId);
        Task GetProgramVersionAsync(int programVersionId);
    }
}