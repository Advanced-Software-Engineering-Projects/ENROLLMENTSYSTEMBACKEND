using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public interface IAdminRepository
    {
        Task<Admin> GetByUsernameAsync(string username);
        Task<int> GetRegisteredStudentsCountAsync();
        Task<int> GetActiveCoursesCountAsync();
        Task<int> GetPendingApprovalsCountAsync();
        Task<List<PendingRequest>> GetPendingRequestsAsync();
        Task<List<EnrollmentData>> GetEnrollmentDataAsync();
        Task<List<CompletionRate>> GetCompletionRateDataAsync();
        Task<List<Student>> GetAllStudentsAsync();
        Task<List<Hold>> GetHoldsByStudentIdAsync(string studentId);
        Task AddHoldAsync(Hold hold);
        Task SetRegistrationPeriodAsync(RegistrationPeriod period);
        Task CloseRegistrationAsync();
    }
}