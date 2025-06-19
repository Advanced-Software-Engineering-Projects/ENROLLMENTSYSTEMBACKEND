using ENROLLMENTSYSTEMBACKEND.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public interface IFormSubmissionRepository
    {
        Task<List<FormSubmission>> GetReconsiderationFormsAsync();
        Task<List<FormSubmission>> GetCompassionateAegrotatFormsAsync();
        Task<List<FormSubmission>> GetCompletionProgrammeFormsAsync();
        Task<FormSubmission?> GetReconsiderationFormByIdAsync(string id);
        Task<FormSubmission?> GetCompassionateAegrotatFormByIdAsync(string id);
        Task<FormSubmission?> GetCompletionProgrammeFormByIdAsync(string id);
        Task<bool> UpdateGradeAsync(string submissionId, string newGrade);
        Task<FormSubmission?> GetFormSubmissionByIdAsync(string submissionId);
        Task<List<FormSubmission>> GetFormSubmissionsByStudentIdAsync(string studentId);
        Task<bool> CreateFormSubmissionAsync(FormSubmission form);
        Task<bool> UpdateFormSubmissionAsync(FormSubmission form);
        Task<bool> DeleteFormSubmissionAsync(string submissionId);
    }
}