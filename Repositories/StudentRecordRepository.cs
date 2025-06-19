using ENROLLMENTSYSTEMBACKEND.Models;
using ENROLLMENTSYSTEMBACKEND.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public class StudentRecordRepository : IStudentRecordRepository
    {
        private readonly EnrollmentInformationDbContext _context;

        public StudentRecordRepository(EnrollmentInformationDbContext context)
        {
            _context = context;
        }

        public async Task<Student> GetStudentByIdAsync(string studentId)
        {
            return await _context.Students
                .FirstOrDefaultAsync(s => s.Id == studentId);
        }

        public async Task<(List<Student> students, int totalCount)> GetStudentRecordsAsync(int page, int pageSize)
        {
            var query = _context.Students.AsQueryable();
            var totalCount = await query.CountAsync();
            var students = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (students, totalCount);
        }

        public async Task UpdateStudentAsync(Student student)
        {
            _context.Students.Update(student);
            await _context.SaveChangesAsync();
        }

        public async Task AddStudentAsync(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
        }
    }
}