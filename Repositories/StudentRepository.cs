using Microsoft.EntityFrameworkCore;
using ENROLLMENTSYSTEMBACKEND.Data;
using ENROLLMENTSYSTEMBACKEND.IRepositories;
using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly StudentInformationDbContext _context;

        public StudentRepository(StudentInformationDbContext context) => _context = context;

        public async Task<Student> GetStudentByIdAsync(string studentId) => await _context.Students.FindAsync(studentId);
        public async Task<IEnumerable<Student>> GetAllStudentsAsync() => await _context.Students.ToListAsync();
        public async Task AddStudentAsync(Student student) { await _context.Students.AddAsync(student); await _context.SaveChangesAsync(); }
        public async Task UpdateStudentAsync(Student student) { _context.Students.Update(student); await _context.SaveChangesAsync(); }
        public async Task DeleteStudentAsync(string studentId)
        {
            var student = await GetStudentByIdAsync(studentId);
            if (student != null) { _context.Students.Remove(student); await _context.SaveChangesAsync(); }
        }
    }
}