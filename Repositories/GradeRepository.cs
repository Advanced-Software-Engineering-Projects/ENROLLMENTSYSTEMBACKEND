using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
   public class GradeRepository : IGradeRepository
    {
        private readonly List<Grade> _grades = new List<Grade>();

        public async Task<List<Grade>> GetGradesByStudentIdAsync(string studentId)
        {
            return await Task.FromResult(_grades.Where(g => g.StudentId == studentId).ToList());
        }

        public async Task UpdateGradeAsync(string studentId, string courseId, string newGrade)
        {
            var grade = _grades.FirstOrDefault(g => g.StudentId == studentId && g.CourseId == courseId);
            if (grade != null)
            {
                grade.GradeValue = newGrade;
            }
            else
            {
                _grades.Add(new Grade { StudentId = studentId, CourseId = courseId, GradeValue = newGrade });
            }
            await Task.CompletedTask;
        }

        public async Task<Grade?> GetGradeAsync(string studentId, string courseCode)
        {
            return await Task.FromResult(_grades.FirstOrDefault(g => g.StudentId == studentId && g.CourseId == courseCode));
        }
    }
}