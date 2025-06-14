using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public interface IServiceHoldRepository
    {
        Task<List<Student>> GetAllStudentsAsync();
        Task<Student> GetStudentByIdAsync(string studentId);
        Task<List<ServiceHold>> GetHoldsByStudentIdAsync(string studentId);
        Task<ServiceHold> AddHoldAsync(ServiceHold hold);
        Task<bool> RemoveHoldAsync(string holdId);
    }
}