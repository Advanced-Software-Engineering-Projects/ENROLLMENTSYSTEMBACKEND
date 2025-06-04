﻿using ENROLLMENTSYSTEMBACKEND.Models;
using ENROLLMENTSYSTEMBACKEND.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly EnrollmentInfromation _context;

        public EnrollmentRepository(EnrollmentInfromation context)
        {
            _context = context;
        }

        public async Task<List<Enrollment>> GetAllEnrollmentsAsync()
        {
            return await _context.Enrollments.ToListAsync();
        }

        public async Task<List<Enrollment>> GetEnrollmentsAsync()
        {
            return await _context.Enrollments.ToListAsync();
        }

        public async Task<List<Enrollment>> GetEnrollmentsByStudentIdAsync(string studentId)
        {
            return await _context.Enrollments.Where(e => e.StudentId == studentId).ToListAsync();
        }

        public async Task<List<Enrollment>> GetEnrollmentsByCourseIdAsync(string courseId)
        {
            return await _context.Enrollments.Where(e => e.CourseId == courseId).ToListAsync();
        }

        public async Task<Enrollment> GetEnrollmentByIdAsync(string enrollmentId)
        {
            return await _context.Enrollments.FirstOrDefaultAsync(e => e.EnrollmentId == enrollmentId);
        }

        public async Task AddEnrollmentAsync(Enrollment enrollment)
        {
            enrollment.EnrollmentId = Guid.NewGuid().ToString();
            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEnrollmentAsync(Enrollment enrollment)
        {
            var existing = await _context.Enrollments.FirstOrDefaultAsync(e => e.EnrollmentId == enrollment.EnrollmentId);
            if (existing != null)
            {
                existing.Status = enrollment.Status;
                // Update other properties if needed
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteEnrollmentAsync(string enrollmentId)
        {
            var enrollment = await _context.Enrollments.FirstOrDefaultAsync(e => e.EnrollmentId == enrollmentId);
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
    }
}
