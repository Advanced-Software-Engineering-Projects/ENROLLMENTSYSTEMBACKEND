using ENROLLMENTSYSTEMBACKEND.DTOs;
using System.Threading.Tasks;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public interface IProgramService
    {
        Task<ProgramAuditDto> GetProgramAuditAsync(string studentId);
        Task<StudentDto?> GetStudentByIdAsync(string studentId);
    }
}
