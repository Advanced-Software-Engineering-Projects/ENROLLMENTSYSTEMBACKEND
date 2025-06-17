using System.Collections.Generic;
using System.Threading.Tasks;
using ExternalFormsService.Models;

namespace ExternalFormsService.Repositories
{
    public interface IExternalFormRepository
    {
        Task<FormSubmission> GetFormByIdAsync(string formId);
        Task<IEnumerable<FormSubmission>> GetFormsByStudentIdAsync(string studentId);
        Task<FormSubmission> AddFormAsync(FormSubmission form);
        Task<FormSubmission> UpdateFormAsync(FormSubmission form);
        Task<bool> DeleteFormAsync(string formId);
    }
} 