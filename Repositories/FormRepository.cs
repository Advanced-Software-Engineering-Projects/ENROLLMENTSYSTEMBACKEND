using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public class FormRepository : IFormRepository
    {
        private readonly List<FormSubmission> _forms = new List<FormSubmission>();

        public async Task<List<FormSubmission>> GetFormsAsync(string? studentId, string? formType)
        {
            var query = _forms.AsQueryable();

            if (!string.IsNullOrEmpty(studentId))
            {
                query = query.Where(f => f.StudentId == studentId);
            }

            if (!string.IsNullOrEmpty(formType))
            {
                query = query.Where(f => f.FormType == formType);
            }

            return await Task.FromResult(query.ToList());
        }

        public async Task<List<FormSubmission>> GetFormsByStudentIdAsync(string studentId)
        {
            return await Task.FromResult(_forms.Where(f => f.StudentId == studentId).ToList());
        }

        public async Task<FormSubmission> GetFormByIdAsync(string formId)
        {
            return await Task.FromResult(_forms.FirstOrDefault(f => f.SubmissionId == formId));
        }

        public async Task<FormSubmission> CreateFormAsync(FormSubmission form)
        {
            form.SubmissionId = Guid.NewGuid().ToString();
            form.SubmissionDate = DateTime.UtcNow;
            _forms.Add(form);
            return await Task.FromResult(form);
        }

        public async Task AddFormAsync(FormSubmission form)
        {
            form.SubmissionId = Guid.NewGuid().ToString();
            form.SubmissionDate = DateTime.UtcNow;
            form.Status = "Submitted";
            _forms.Add(form);
            await Task.CompletedTask;
        }

        public async Task UpdateFormStatusAsync(string formId, string status)
        {
            var form = _forms.FirstOrDefault(f => f.SubmissionId == formId);
            if (form != null)
            {
                form.Status = status;
            }
            await Task.CompletedTask;
        }
    }
}