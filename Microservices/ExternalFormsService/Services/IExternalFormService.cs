using System.Collections.Generic;
using System.Threading.Tasks;
using ExternalFormsService.DTOs;

namespace ExternalFormsService.Services
{
    public interface IExternalFormService
    {
        Task<ExternalFormSubmissionDto> GetFormByIdAsync(string formId);
        Task<IEnumerable<ExternalFormSubmissionDto>> GetStudentFormsAsync(string studentId);
        Task<ExternalFormSubmissionDto> SubmitFormAsync(string studentId, string formType, FormDataDto formData);
        Task<ExternalFormSubmissionDto> UpdateFormStatusAsync(string formId, string status, string comments, string updatedBy);
        Task<bool> DeleteFormAsync(string formId);
        Task<bool> CheckEligibilityAsync(string studentId, string formType);
    }
} 