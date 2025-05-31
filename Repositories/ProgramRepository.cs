using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public class ProgramRepository : IProgramRepository
    {
        private readonly List<Programs> _programs = new List<Programs>
        {
            new Programs { Id = "P1", Name = "Computer Science", RequiredCourseIds = new List<string> { "C1", "C2", "C3" } }
        };

        public async Task<Programs> GetProgramByIdAsync(string programId)
        {
            return await Task.FromResult(_programs.FirstOrDefault(p => p.Id == programId));
        }
    }
}