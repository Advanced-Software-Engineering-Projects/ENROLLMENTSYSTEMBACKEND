using ENROLLMENTSYSTEMBACKEND.Models;
using ENROLLMENTSYSTEMBACKEND.Data;
using Microsoft.EntityFrameworkCore;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly EnrollmentInfromation _context;

        public StudentRepository(EnrollmentInfromation context)
        {
            _context = context;
        }

        public async Task<List<Student>> GetStudentsAsync()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<Student> GetStudentByIdAsync(string id)
        {
            return await _context.Students.FirstOrDefaultAsync(s => s.StudentId == id);
        }

        public async Task UpdateStudentAsync(Student student)
        {
            var existingStudent = await _context.Students.FirstOrDefaultAsync(s => s.StudentId == student.StudentId);
            if (existingStudent != null)
            {
                existingStudent.Name = student.Name;
                existingStudent.Email = student.Email;
                existingStudent.AvatarUrl = student.AvatarUrl;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Student>> GetAllStudentsAsync()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<int> GetRegisteredStudentsCountAsync()
        {
            return await _context.Students.CountAsync();
        }

        public async Task<List<Student>> GetAllStudentsAsync(int page, int pageSize)
        {
            var skip = (page - 1) * pageSize;
            return await _context.Students.Skip(skip).Take(pageSize).ToListAsync();
        }

        public async Task<int> GetTotalStudentsCountAsync()
        {
            return await _context.Students.CountAsync();
        }
    }
}
