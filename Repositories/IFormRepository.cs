using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public interface IFormRepository
    {
        Task AddSubmissionAsync(FormSubmission submission);
        Task<List<FormSubmission>> GetSubmissionsByTypeAsync(string formType);
    }
}
