using ENROLLMENTSYSTEMBACKEND.Models;
using ENROLLMENTSYSTEMBACKEND.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly EnrollmentInformationDbContext _context;

        public EnrollmentRepository(EnrollmentInformationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Enrollment>> GetAllEnrollmentsAsync()
        {
            return await _context.Enrollments
                .Include(e => e.Course)
                .ToListAsync();
        }

        public async Task<List<Enrollment>> GetEnrollmentsAsync()
        {
            return await _context.Enrollments
                .Include(e => e.Course)
                .ToListAsync();
        }

        public async Task<List<Enrollment>> GetEnrollmentsByStudentIdAsync(string studentId)
        {
            return await _context.Enrollments
                .Include(e => e.Course)
                .Where(e => e.StudentId == studentId)
                .ToListAsync();
        }

        public async Task<List<Enrollment>> GetEnrollmentsByCourseIdAsync(string courseId)
        {
            return await _context.Enrollments
                .Include(e => e.Course)
                .Where(e => e.CourseId == courseId)
                .ToListAsync();
        }

        public async Task<Enrollment> GetEnrollmentByIdAsync(string enrollmentId)
        {
            return await _context.Enrollments
                .Include(e => e.Course)
                .FirstOrDefaultAsync(e => e.EnrollmentId == enrollmentId);
        }

        public async Task AddEnrollmentAsync(Enrollment enrollment)
        {
            if (string.IsNullOrEmpty(enrollment.EnrollmentId))
            {
                enrollment.EnrollmentId = Guid.NewGuid().ToString();
            }
            enrollment.EnrollmentDate = DateTime.UtcNow;
            
            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEnrollmentAsync(Enrollment enrollment)
        {
            var existing = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.EnrollmentId == enrollment.EnrollmentId);
            
            if (existing != null)
            {
                // Update all relevant fields
                existing.Status = enrollment.Status;
                existing.Grade = enrollment.Grade;
                existing.CompletionDate = enrollment.CompletionDate ?? 
                    (!string.IsNullOrEmpty(enrollment.Grade) ? DateTime.UtcNow : existing.CompletionDate);
                existing.Semester = enrollment.Semester;
                existing.Year = enrollment.Year;

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteEnrollmentAsync(string enrollmentId)
        {
            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.EnrollmentId == enrollmentId);
            
            if (enrollment != null)
            {
                _context.Enrollments.Remove(enrollment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<(string Semester, int Count)>> GetEnrollmentCountsBySemesterAsync()
        {
            return await _context.Enrollments
                .GroupBy(e => e.Semester)
                .Select(g => new ValueTuple<string, int>(g.Key, g.Count()))
                .ToListAsync();
        }

        public async Task<List<Enrollment>> GetEnrollmentsBySemesterAsync(string studentId, string semester)
        {
            return await _context.Enrollments
                .Include(e => e.Course)
                .Where(e => e.StudentId == studentId && e.Semester == semester)
                .ToListAsync();
        }

        public async Task UpdateGradeAsync(string enrollmentId, string grade)
        {
            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.EnrollmentId == enrollmentId);

            if (enrollment != null)
            {
                enrollment.Grade = grade;
                enrollment.CompletionDate = DateTime.UtcNow;
                enrollment.Status = "Completed";
                
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("Enrollment not found.");
            }
        }

        public async Task<List<Enrollment>> GetCompletedEnrollmentsAsync(string studentId)
        {
            return await _context.Enrollments
                .Include(e => e.Course)
                .Where(e => e.StudentId == studentId && 
                           !string.IsNullOrEmpty(e.Grade) && 
                           e.Grade != "F")
                .ToListAsync();
        }
    }
}