using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public interface IFormRepository
    {
        Task<List<FormSubmission>> GetFormsAsync(string? studentId, string? formType);
        Task<FormSubmission> GetFormByIdAsync(string formId);
        Task<FormSubmission> CreateFormAsync(FormSubmission form);
        Task UpdateFormStatusAsync(string formId, string status);
        Task<List<FormSubmission>> GetFormsByStudentIdAsync(string studentId);
        Task AddFormAsync(FormSubmission form);
    }
}
