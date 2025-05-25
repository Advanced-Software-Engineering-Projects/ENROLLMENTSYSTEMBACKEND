using ENROLLMENTSYSTEMBACKEND.Models;
using Microsoft.EntityFrameworkCore;
using StudentSystemBackend.Repositories;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly StudentInformationDbContext _context;

        public StudentRepository(StudentInformationDbContext context)
        {
            _context = context;
        }

        public async Task<Student> GetByIdAsync(string id)
        {
            return await _context.Students
                .Include(s => s.Enrollments)
                .ThenInclude(e => e.Course)
                .FirstOrDefaultAsync(s => s.StudentId == id);
        }

        public async Task<Student> GetByEmailAsync(string email)
        {
            return await _context.Students
                .FirstOrDefaultAsync(s => s.Email == email);
        }

        public async Task<List<Enrollment>> GetEnrollmentsByStudentIdAsync(string studentId)
        {
            return await _context.Enrollments
                .Include(e => e.Course)
                .Where(e => e.StudentId == studentId)
                .ToListAsync();
        }

        public async Task<List<Student>> GetAllAsync()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task UpdateAsync(Student student)
        {
            _context.Students.Update(student);
            await _context.SaveChangesAsync();
        }
    }
}