using ENROLLMENTSYSTEMBACKEND.Models;
using ENROLLMENTSYSTEMBACKEND.Data;
using Microsoft.EntityFrameworkCore;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public class PrerequisiteRepository : IPrerequisiteRepository
    {
        private readonly EnrollmentInformationDbContext _context;

        public PrerequisiteRepository(EnrollmentInformationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Prerequisite>> GetPrerequisitesAsync(string courseCode)
        {
            try
            {
                return await _context.Prerequisites
                    .Where(p => p.CourseCode == courseCode)
                    .ToListAsync();
            }
            catch (Exception)
            {
                // Return empty list if there's an error or no prerequisites found
                return new List<Prerequisite>();
            }
        }

        public async Task<List<Prerequisite>> GetPrerequisitesForCourseAsync(string courseCode)
        {
            return await GetPrerequisitesAsync(courseCode);
        }
    }
}
