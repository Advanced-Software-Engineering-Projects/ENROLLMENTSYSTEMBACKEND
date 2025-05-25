using ENROLLMENTSYSTEMBACKEND.DTOs;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public interface IProgramService
    {
        Task GenerateAcademicPlanAsync(string studentId);
        Task<ProgramAuditDto> GetProgramAuditAsync(string studentId);
    }
}
