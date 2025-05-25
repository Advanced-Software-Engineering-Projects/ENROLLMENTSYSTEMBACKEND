using ENROLLMENTSYSTEMBACKEND.Models;
using Microsoft.EntityFrameworkCore;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly CourseManagementDbContext _context;

        public EnrollmentRepository(CourseManagementDbContext context)
        {
            _context = context;
        }

        public async Task AddEnrollmentAsync(string studentId, int courseId, string semester)
        {
            var enrollment = new Enrollment
            {
                StudentId = studentId,
                CourseId = courseId,
                Semester = semester
            };
            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveEnrollmentAsync(string studentId, int courseId, string semester)
        {
            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId && e.Semester == semester);
            if (enrollment != null)
            {
                _context.Enrollments.Remove(enrollment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Enrollment>> GetEnrollmentsByStudentAsync(string studentId)
        {
            return await _context.Enrollments
                .Where(e => e.StudentId == studentId)
                .ToListAsync();
        }

        public async Task<List<Course>> GetCoursesBySemesterAsync(string semester)
        {
            return await _context.Courses
                .Where(c => c.SemesterOffered == semester && c.IsActive)
                .ToListAsync();
        }
    }
}