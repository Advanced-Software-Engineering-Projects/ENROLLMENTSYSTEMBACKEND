using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.IRepositories
{
    public interface IProgramRepository
    {
        Task<Programs> GetProgramByIdAsync(int programId);
        Task<IEnumerable<Programs>> GetAllProgramsAsync();
        Task AddProgramAsync(Programs program);
        Task UpdateProgramAsync(Programs program);
        Task DeleteProgramAsync(int programId);
    }
}
