using Microsoft.EntityFrameworkCore;
using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public class ProgramRepository : IProgramRepository
    {
        private readonly CourseManagementDbContext _context;

        public ProgramRepository(CourseManagementDbContext context)
        {
            _context = context;
        }

        public async Task<Programs> GetByIdAsync(int id)
        {
            return await _context.Programs
                .Include(p => p.ProgramVersions)
                .FirstOrDefaultAsync(p => p.ProgramId == id);
        }

        public async Task<List<ProgramCourse>> GetProgramCoursesAsync(int programVersionId)
        {
            return await _context.ProgramCourses
                .Where(pc => pc.ProgramVersionId == programVersionId)
                .Include(pc => pc.Course)
                .ToListAsync();
        }

        public async Task<ProgramVersion> GetProgramVersionAsync(int programVersionId)
        {
            return await _context.ProgramVersions
                .FirstOrDefaultAsync(pv => pv.ProgramVersionId == programVersionId);
        }

        Task IProgramRepository.GetProgramVersionAsync(int programVersionId)
        {
            return GetProgramVersionAsync(programVersionId);
        }
    }
}