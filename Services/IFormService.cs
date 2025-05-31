using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public interface IFormService
    {
        Task<List<FormSubmissionDto>> GetFormsAsync(string? studentId, string? formType);
        Task<FormSubmissionDto> GetFormByIdAsync(string formId);
        Task<FormSubmissionDto> CreateFormAsync(CreateFormDto createFormDto);
        Task<FormSubmissionDto> UpdateFormStatusAsync(UpdateStatusDto updateStatusDto);
        Task<List<FormSubmission>> GetFormsAsync(string studentId);
        Task<FormSubmission> SubmitFormAsync(FormSubmissionDto formDto);
    }
}
