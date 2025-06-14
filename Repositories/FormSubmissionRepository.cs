using ENROLLMENTSYSTEMBACKEND.Data;
using ENROLLMENTSYSTEMBACKEND.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public class FormSubmissionRepository : IFormSubmissionRepository
    {
        private readonly EnrollmentInformationDbContext _context;

        public FormSubmissionRepository(EnrollmentInformationDbContext context)
        {
            _context = context;
        }

        public async Task<List<FormSubmission>> GetReconsiderationFormsAsync()
        {
            return await _context.FormSubmissions
                .Where(f => f.FormType == "Reconsideration")
                .ToListAsync();
        }

        public async Task<List<FormSubmission>> GetCompassionateAegrotatFormsAsync()
        {
            return await _context.FormSubmissions
                .Where(f => f.FormType == "CompassionateAegrotat")
                .ToListAsync();
        }

        public async Task<List<FormSubmission>> GetCompletionProgrammeFormsAsync()
        {
            return await _context.FormSubmissions
                .Where(f => f.FormType == "CompletionProgramme")
                .ToListAsync();
        }

        public async Task<FormSubmission?> GetReconsiderationFormByIdAsync(string id)
        {
            return await _context.FormSubmissions
                .FirstOrDefaultAsync(f => f.SubmissionId == id && f.FormType == "Reconsideration");
        }

        public async Task<FormSubmission?> GetCompassionateAegrotatFormByIdAsync(string id)
        {
            return await _context.FormSubmissions
                .FirstOrDefaultAsync(f => f.SubmissionId == id && f.FormType == "CompassionateAegrotat");
        }

        public async Task<FormSubmission?> GetCompletionProgrammeFormByIdAsync(string id)
        {
            return await _context.FormSubmissions
                .FirstOrDefaultAsync(f => f.SubmissionId == id && f.FormType == "CompletionProgramme");
        }

        public async Task<bool> UpdateGradeAsync(string submissionId, string newGrade)
        {
            var form = await _context.FormSubmissions.FindAsync(submissionId);
            if (form == null)
                return false;

            form.NewGrade = newGrade;
            _context.FormSubmissions.Update(form);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<FormSubmission?> GetFormSubmissionByIdAsync(string submissionId)
        {
            return await _context.FormSubmissions.FindAsync(submissionId);
        }

        public async Task<List<FormSubmission>> GetFormSubmissionsByStudentIdAsync(string studentId)
        {
            return await _context.FormSubmissions
                .Where(f => f.StudentId == studentId)
                .ToListAsync();
        }

        public async Task<bool> CreateFormSubmissionAsync(FormSubmission form)
        {
            await _context.FormSubmissions.AddAsync(form);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateFormSubmissionAsync(FormSubmission form)
        {
            _context.FormSubmissions.Update(form);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteFormSubmissionAsync(string submissionId)
        {
            var form = await _context.FormSubmissions.FindAsync(submissionId);
            if (form == null)
                return false;

            _context.FormSubmissions.Remove(form);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}