
using ENROLLMENTSYSTEMBACKEND.DTOs;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public interface IFormService
    {
        Task SubmitFormAsync(string formType, FormSubmissionDto formData);
        Task<List<FormSubmissionDto>> GetFormSubmissionsAsync(string formType);
    }
}