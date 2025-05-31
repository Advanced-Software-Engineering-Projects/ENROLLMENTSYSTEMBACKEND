using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public class PrerequisiteRepository : IPrerequisiteRepository
    {
        private readonly List<Prerequisite> _prerequisites = new List<Prerequisite>
        {
            new Prerequisite { CourseCode = "PHYS101", PrerequisiteCourseCode = "MATH101" }
        };

        public async Task<List<Prerequisite>> GetPrerequisitesAsync(string courseCode)
        {
            return await Task.FromResult(_prerequisites.Where(p => p.CourseCode == courseCode).ToList());
        }
    }
}
