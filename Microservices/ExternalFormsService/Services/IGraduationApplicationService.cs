using System.Threading.Tasks;
using ExternalFormsService.DTOs;

namespace ExternalFormsService.Services
{
    public interface IGraduationApplicationService
    {
        Task<GraduationApplicationResponseDto> SubmitGraduationApplicationAsync(GraduationApplicationDto application);
        Task<GraduationApplicationResponseDto> GetApplicationStatusAsync(string applicationId);
        Task<bool> ProcessApplicationAsync(string applicationId, string status, string comments);
        Task<bool> SendNotificationAsync(string studentId, GraduationApplicationResponseDto response);
    }
} 