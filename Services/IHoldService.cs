using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public interface IHoldService
    {
        Task<List<HoldResponseDto>> GetHoldsAsync(string? studentId);
        Task<HoldResponseDto> AddHoldAsync(HoldDto holdDto);
        Task RemoveHoldAsync(string id);
        Task<bool> HasHoldsAsync(string studentId);
        Task<List<string>> GetAvailableServicesAsync();
        Task UpdateStudentHoldServicesAsync(string studentId, List<string> restrictedServices);
        Task<List<string>> GetStudentRestrictedServicesAsync(string studentId);
        Task<bool> CanAccessServiceAsync(string studentId, string service);
    }
}