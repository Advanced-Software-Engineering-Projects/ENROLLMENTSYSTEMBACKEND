using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Models;
using Microsoft.EntityFrameworkCore;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public class FormRepository : IFormRepository
    {
        private readonly StudentInformationDbContext _context;

        public FormRepository(StudentInformationDbContext context)
        {
            _context = context;
        }

        public async Task AddSubmissionAsync(FormSubmission submission)
        {
            await _context.FormSubmissions.AddAsync(submission);
            await _context.SaveChangesAsync();
        }

        public async Task<List<FormSubmission>> GetSubmissionsByTypeAsync(string formType)
        {
            return await _context.FormSubmissions
                .Where(s => s.FormType == formType)
                .ToListAsync();
        }
    }
}