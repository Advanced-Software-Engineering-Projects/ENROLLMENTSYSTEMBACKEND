using ENROLLMENTSYSTEMBACKEND.DTOs;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public interface IServiceHoldService
    {
        Task<List<StudentWithHoldsDto>> GetAllStudentsAsync();
        Task<StudentWithHoldsDto> GetStudentWithHoldsAsync(string studentId);
        Task<ServiceHoldDto> AddHoldAsync(CreateServiceHoldDto holdDto);
        Task<bool> RemoveHoldAsync(string holdId);
        Task<byte[]> GenerateHoldsPdfAsync(string studentId);
    }
}