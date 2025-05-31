using ENROLLMENTSYSTEMBACKEND.DTOs;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public interface IProgramService
    {
        Task<ProgramAuditDto> GetProgramAuditAsync(string studentId);
    }
}
