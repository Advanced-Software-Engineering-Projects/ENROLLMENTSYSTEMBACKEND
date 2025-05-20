using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.IRepositories
{
    public interface IProgramVersionRepository
    {
        Task<ProgramVersion> GetProgramVersionByIdAsync(int programVersionId);
        Task<ProgramVersion> GetProgramVersionByProgramAndYearAsync(int programId, int handbookYear);
        Task<IEnumerable<ProgramVersion>> GetAllProgramVersionsAsync();
        Task AddProgramVersionAsync(ProgramVersion programVersion);
    }
}
