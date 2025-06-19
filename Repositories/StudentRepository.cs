using ENROLLMENTSYSTEMBACKEND.Data;
using ENROLLMENTSYSTEMBACKEND.Models;
using Microsoft.EntityFrameworkCore;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly EnrollmentInformationDbContext _context;

        public StudentRepository(EnrollmentInformationDbContext context)
        {
            _context = context;
        }

        public async Task<Student?> GetStudentByIdAsync(string studentId)
        {
            return await _context.Students
                .FirstOrDefaultAsync(s => s.StudentId == studentId);
        }

        public async Task<List<Enrollment>> GetEnrollmentsAsync(string studentId)
        {
            return await _context.Enrollments
                .Include(e => e.Course)
                .Where(e => e.StudentId == studentId)
                .ToListAsync();
        }

        public async Task<List<Enrollment>> GetActiveEnrollmentsAsync(string studentId)
        {
            var currentSemester = DateTime.Now.Month <= 6 ? "1" : "2";
            var currentYear = DateTime.Now.Year;

            return await _context.Enrollments
                .Include(e => e.Course)
                .Where(e => e.StudentId == studentId &&
                           e.Year == currentYear &&
                           e.Semester == currentSemester)
                .ToListAsync();
        }

        public async Task<bool> IsEligibleForGraduationAsync(string studentId)
        {
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.StudentId == studentId);

            if (student == null)
                return false;

            // Get completed courses for the student
            var completedCourses = await _context.Enrollments
                .Where(e => e.StudentId == studentId && 
                           !string.IsNullOrEmpty(e.Grade) && 
                           e.Grade != "F")
                .Select(e => e.CourseId)
                .ToListAsync();

            // Get required courses for the student's program
            var requiredCourses = await _context.Courses
                .Where(c => c.Program == student.Program)
                .Select(c => c.CourseId)
                .ToListAsync();

            // Check if all required courses are completed
            return requiredCourses.All(rc => completedCourses.Contains(rc));
        }

        public async Task<bool> UpdateStudentAsync(Student student)
        {
            var existing = await _context.Students
                .FirstOrDefaultAsync(s => s.StudentId == student.StudentId);

            if (existing == null)
                return false;

            _context.Entry(existing).CurrentValues.SetValues(student);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<List<Student>> GetAllStudentsAsync()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<List<Student>> GetAllStudentsAsync(int page, int pageSize)
        {
            return await _context.Students
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalStudentsCountAsync()
        {
            return await _context.Students.CountAsync();
        }

        public async Task<int> GetRegisteredStudentsCountAsync()
        {
            var currentSemester = DateTime.Now.Month <= 6 ? "1" : "2";
            var currentYear = DateTime.Now.Year;

            return await _context.Enrollments
                .Where(e => e.Year == currentYear && e.Semester == currentSemester)
                .Select(e => e.StudentId)
                .Distinct()
                .CountAsync();
        }
    }
}