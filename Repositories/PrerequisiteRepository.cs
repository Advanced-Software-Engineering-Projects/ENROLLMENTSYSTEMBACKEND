using ENROLLMENTSYSTEMBACKEND.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public class PrerequisiteRepository : IPrerequisiteRepository
    {
        private readonly CourseManagementDbContext _context;

        public PrerequisiteRepository(CourseManagementDbContext context)
        {
            _context = context;
        }

        public async Task<List<PrerequisiteDto>> GetPrerequisitesAsync(int courseId)
        {
            return await _context.Prerequisites
                .Where(p => p.CourseId == courseId)
                .Select(p => new PrerequisiteDto
                {
                    PrerequisiteCourseId = p.PrerequisiteCourseId,
                    Code = p.PrerequisiteCourse.Code,
                    Name = p.PrerequisiteCourse.Name
                })
                .ToListAsync();
        }

        public async Task<PrerequisiteGraphDto> GetPrerequisiteGraphAsync()
        {
            var allPrerequisites = await _context.Prerequisites
                .Include(p => p.PrerequisiteCourse)
                .ToListAsync();

            var graph = allPrerequisites
                .GroupBy(p => p.CourseId)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(p => p.PrerequisiteCourseId).ToList()
                );

            return new PrerequisiteGraphDto { Graph = graph };
        }
    }
}