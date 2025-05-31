using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly List<Student> _students = new List<Student>(); 

        public async Task<List<Student>> GetStudentsAsync()
        {
            return await Task.FromResult(_students);
        }

        public async Task<Student> GetStudentByIdAsync(string id)
        {
            return await Task.FromResult(_students.FirstOrDefault(s => s.StudentId == id));
        }

        public async Task UpdateStudentAsync(Student student)
        {
            var existingStudent = _students.FirstOrDefault(s => s.StudentId == student.StudentId);
            if (existingStudent != null)
            {
                existingStudent.Name = student.Name;
                existingStudent.Email = student.Email;
                existingStudent.AvatarUrl = student.AvatarUrl;
            }
            await Task.CompletedTask;
        }

        public async Task<List<Student>> GetAllStudentsAsync()
        {
            return await Task.FromResult(_students);
        }

        public async Task<int> GetRegisteredStudentsCountAsync()
        {
            return await Task.FromResult(_students.Count);
        }

        public async Task<List<Student>> GetAllStudentsAsync(int page, int pageSize)
        {
            var skip = (page - 1) * pageSize;
            return await Task.FromResult(_students.Skip(skip).Take(pageSize).ToList());
        }

        public async Task<int> GetTotalStudentsCountAsync()
        {
            return await Task.FromResult(_students.Count);
        }
    }
}