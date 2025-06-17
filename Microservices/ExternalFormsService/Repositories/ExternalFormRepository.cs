using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExternalFormsService.Data;
using ExternalFormsService.Models;
using Microsoft.EntityFrameworkCore;

namespace ExternalFormsService.Repositories
{
    public class ExternalFormRepository : IExternalFormRepository
    {
        private readonly ExternalFormsDbContext _context;

        public ExternalFormRepository(ExternalFormsDbContext context)
        {
            _context = context;
        }

        public async Task<FormSubmission> GetFormByIdAsync(string formId)
        {
            return await _context.FormSubmissions.FindAsync(formId);
        }

        public async Task<IEnumerable<FormSubmission>> GetFormsByStudentIdAsync(string studentId)
        {
            return await _context.FormSubmissions
                .Where(f => f.StudentId == studentId)
                .OrderByDescending(f => f.SubmissionDate)
                .ToListAsync();
        }

        public async Task<FormSubmission> AddFormAsync(FormSubmission form)
        {
            await _context.FormSubmissions.AddAsync(form);
            await _context.SaveChangesAsync();
            return form;
        }

        public async Task<FormSubmission> UpdateFormAsync(FormSubmission form)
        {
            _context.FormSubmissions.Update(form);
            await _context.SaveChangesAsync();
            return form;
        }

        public async Task<bool> DeleteFormAsync(string formId)
        {
            var form = await _context.FormSubmissions.FindAsync(formId);
            if (form == null)
            {
                return false;
            }

            _context.FormSubmissions.Remove(form);
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 