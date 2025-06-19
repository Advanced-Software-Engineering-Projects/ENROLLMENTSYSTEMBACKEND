using ENROLLMENTSYSTEMBACKEND.Data;
using ENROLLMENTSYSTEMBACKEND.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public class FormRepository : IFormRepository
    {
        private readonly EnrollmentInformationDbContext _context;

        public FormRepository(EnrollmentInformationDbContext context)
        {
            _context = context;
        }

        public async Task<List<FormSubmission>> GetFormsAsync(string? studentId, string? formType)
        {
            var query = _context.FormSubmissions.AsQueryable();

            if (!string.IsNullOrEmpty(studentId))
            {
                query = query.Where(f => f.StudentId == studentId);
            }

            if (!string.IsNullOrEmpty(formType))
            {
                query = query.Where(f => f.FormType == formType);
            }

            return await query.ToListAsync();
        }

        public async Task<List<FormSubmission>> GetFormsByStudentIdAsync(string studentId)
        {
            return await _context.FormSubmissions
                .Where(f => f.StudentId == studentId)
                .ToListAsync();
        }

        public async Task<FormSubmission> GetFormByIdAsync(string formId)
        {
            return await _context.FormSubmissions
                .FirstOrDefaultAsync(f => f.SubmissionId == formId);
        }

        public async Task<FormSubmission> CreateFormAsync(FormSubmission form)
        {
            form.SubmissionId = System.Guid.NewGuid().ToString();
            form.SubmissionDate = System.DateTime.UtcNow;
            _context.FormSubmissions.Add(form);
            await _context.SaveChangesAsync();
            return form;
        }

        public async Task AddFormAsync(FormSubmission form)
        {
            form.SubmissionId = System.Guid.NewGuid().ToString();
            form.SubmissionDate = System.DateTime.UtcNow;
            form.Status = "Submitted";
            _context.FormSubmissions.Add(form);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateFormStatusAsync(string formId, string status)
        {
            var form = await _context.FormSubmissions
                .FirstOrDefaultAsync(f => f.SubmissionId == formId);
            if (form != null)
            {
                form.Status = status;
                await _context.SaveChangesAsync();
            }
        }
    }
}