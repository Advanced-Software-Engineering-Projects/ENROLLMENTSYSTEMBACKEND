using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Models;
using System.Threading.Tasks;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public interface IFormConfigurationService
    {
        Task<FormSubmissionsResponseDto> GetAllFormSubmissionsAsync();
        Task<bool> UpdateGradeAsync(GradeUpdateDto gradeUpdateDto);
        Task<bool> UpdateStatusAsync(StatusUpdateDto statusUpdateDto, string formType);
        Task<bool> ResendEmailAsync(EmailResendDto emailResendDto);
        Task<bool> SendFormSubmissionEmailAsync(string formType, string submissionId);
        Task<bool> ResendFormSubmissionEmailAsync(string formType, string submissionId, string email);
        FormSubmissionDto MapToFormSubmissionDto(FormSubmission form);
    }
}