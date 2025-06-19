using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public interface IStudentRecordRepository
    {
        Task<(List<Student> students, int totalCount)> GetStudentRecordsAsync(int page, int pageSize);
        Task<Student> GetStudentByIdAsync(string studentId);
    }
}