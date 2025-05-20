using Microsoft.EntityFrameworkCore;
using ENROLLMENTSYSTEMBACKEND.Data;
using ENROLLMENTSYSTEMBACKEND.IRepositories;
using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public class PrerequisiteRepository : IPrerequisiteRepository
    {
        private readonly CourseManagementDbContext _context;

        public PrerequisiteRepository(CourseManagementDbContext context) => _context = context;

        public async Task<IEnumerable<Prerequisite>> GetPrerequisitesForCourseAsync(int courseId)
            => await _context.Prerequisites.Where(p => p.CourseId == courseId).ToListAsync();

        public async Task AddPrerequisiteAsync(Prerequisite prerequisite)
        {
            await _context.Prerequisites.AddAsync(prerequisite);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Prerequisite>> GetPrerequisitesForCoursesAsync(IEnumerable<int> courseIds)
            => await _context.Prerequisites.Where(p => courseIds.Contains(p.CourseId)).ToListAsync();
    }
}