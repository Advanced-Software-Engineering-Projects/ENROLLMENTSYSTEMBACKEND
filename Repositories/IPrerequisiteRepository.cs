using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public interface IPrerequisiteRepository
    {
        Task<List<PrerequisiteDto>> GetPrerequisitesAsync(int courseId);
        Task<PrerequisiteGraphDto> GetPrerequisiteGraphAsync();
    }
}
