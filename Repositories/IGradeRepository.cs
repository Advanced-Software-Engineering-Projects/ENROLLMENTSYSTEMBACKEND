using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public interface IGradeRepository
    {
        Task<List<Grade>> GetGradesByStudentIdAsync(string studentId);
        Task UpdateGradeAsync(string studentId, string courseId, string newGrade);
    }
}
