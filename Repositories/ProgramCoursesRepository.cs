using Microsoft.EntityFrameworkCore;
using ENROLLMENTSYSTEMBACKEND.Data;
using ENROLLMENTSYSTEMBACKEND.IRepositories;
using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public class ProgramCoursesRepository : IProgramCoursesRepository
    {
        private readonly CourseManagementDbContext _context;

        public ProgramCoursesRepository(CourseManagementDbContext context) => _context = context;

        public async Task<IEnumerable<ProgramCourses>> GetProgramCoursesByProgramVersionAsync(int programVersionId) => await _context.ProgramCourses.Where(pc => pc.ProgramVersionId == programVersionId).ToListAsync();
        public async Task AddProgramCourseAsync(ProgramCourses programCourse) { await _context.ProgramCourses.AddAsync(programCourse); await _context.SaveChangesAsync(); }
        public async Task DeleteProgramCourseAsync(int programCourseId)
        {
            var pc = await _context.ProgramCourses.FindAsync(programCourseId);
            if (pc != null) { _context.ProgramCourses.Remove(pc); await _context.SaveChangesAsync(); }
        }
    }
}