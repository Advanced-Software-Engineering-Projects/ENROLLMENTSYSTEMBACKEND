using ENROLLMENTSYSTEMBACKEND.DTOs;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public interface IHoldService
    {
        Task<List<HoldResponseDto>> GetHoldsAsync(string? studentId);
        Task<HoldResponseDto> AddHoldAsync(HoldDto holdDto);
        Task RemoveHoldAsync(string id);
    }
}
