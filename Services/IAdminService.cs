using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public interface IAdminService
    {
        Task<AdminDashboardDto> GetDashboardDataAsync();
        Task<List<StudentDto>> GetAllStudentsAsync();
        Task<List<HoldDto>> GetHoldsAsync(string studentId);
        Task AddHoldAsync(HoldDto hold);
        Task OpenRegistrationAsync(RegistrationPeriodDto period);
        Task CloseRegistrationAsync();   
    }
}
