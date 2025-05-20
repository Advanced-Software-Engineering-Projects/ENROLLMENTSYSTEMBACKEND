using Microsoft.EntityFrameworkCore;
using ENROLLMENTSYSTEMBACKEND.Data;
using ENROLLMENTSYSTEMBACKEND.IRepositories;
using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public class ProgramVersionRepository : IProgramVersionRepository
    {
        private readonly CourseManagementDbContext _context;

        public ProgramVersionRepository(CourseManagementDbContext context) => _context = context;

        public async Task<ProgramVersion> GetProgramVersionByIdAsync(int programVersionId) => await _context.ProgramVersions.FindAsync(programVersionId);
        public async Task<ProgramVersion> GetProgramVersionByProgramAndYearAsync(int programId, int handbookYear) => await _context.ProgramVersions.FirstOrDefaultAsync(pv => pv.ProgramId == programId && pv.HandbookYear == handbookYear);
        public async Task<IEnumerable<ProgramVersion>> GetAllProgramVersionsAsync() => await _context.ProgramVersions.ToListAsync();
        public async Task AddProgramVersionAsync(ProgramVersion programVersion) { await _context.ProgramVersions.AddAsync(programVersion); await _context.SaveChangesAsync(); }
    }
}