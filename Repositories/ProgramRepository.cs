using Microsoft.EntityFrameworkCore;
using ENROLLMENTSYSTEMBACKEND.Data;
using ENROLLMENTSYSTEMBACKEND.IRepositories;
using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public class ProgramRepository : IProgramRepository
    {
        private readonly CourseManagementDbContext _context;

        public ProgramRepository(CourseManagementDbContext context) => _context = context;

        public async Task<Programs> GetProgramByIdAsync(int programId) => await _context.Programs.FindAsync(programId);
        public async Task<IEnumerable<Programs>> GetAllProgramsAsync() => await _context.Programs.ToListAsync();
        public async Task AddProgramAsync(Programs program) { await _context.Programs.AddAsync(program); await _context.SaveChangesAsync(); }
        public async Task UpdateProgramAsync(Programs program) { _context.Programs.Update(program); await _context.SaveChangesAsync(); }
        public async Task DeleteProgramAsync(int programId)
        {
            var program = await GetProgramByIdAsync(programId);
            if (program != null) { _context.Programs.Remove(program); await _context.SaveChangesAsync(); }
        }
    }
}