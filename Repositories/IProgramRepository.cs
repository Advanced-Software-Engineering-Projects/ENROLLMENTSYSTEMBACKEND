using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public interface IProgramRepository
    {
        Task<Programs> GetProgramByIdAsync(string programId);
    }
}
