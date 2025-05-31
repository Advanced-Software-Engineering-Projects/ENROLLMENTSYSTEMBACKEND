using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public interface IPrerequisiteRepository
    {
        Task<List<Prerequisite>> GetPrerequisitesAsync(string courseCode);
    }
}
