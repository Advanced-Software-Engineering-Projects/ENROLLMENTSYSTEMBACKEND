using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ENROLLMENTSYSTEMBACKEND.Models;
using ENROLLMENTSYSTEMBACKEND.Data;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public class GradeRecheckService : IGradeRecheckService
    {
        private readonly EnrollmentInformationDbContext _context;
        private readonly IEmailService _emailService;
        private readonly IGradeService _gradeService;

        public GradeRecheckService(
            EnrollmentInformationDbContext context,
            IEmailService emailService,
            IGradeService gradeService)
        {
            _context = context;
            _emailService = emailService;
            _gradeService = gradeService;
        }

        public async Task<GradeRecheckRequest> CreateRecheckRequestAsync(string studentId, string courseId, string reason)
        {
            var currentGrade = await _context.Grades
                .FirstOrDefaultAsync(g => g.StudentId == studentId && g.CourseId == courseId);

            if (currentGrade == null)
                throw new InvalidOperationException("No grade found for this course");

            var request = new GradeRecheckRequest
            {
                StudentId = studentId,
                CourseId = courseId,
                CurrentGrade = currentGrade.GradeValue,
                Reason = reason,
                Status = "Pending"
            };

            _context.GradeRecheckRequests.Add(request);
            await _context.SaveChangesAsync();

            // Notify admin about new recheck request
            await _emailService.SendEmailAsync(
                "admin@university.edu",
                "New Grade Recheck Request",
                $"A new grade recheck request has been submitted for student {studentId} in course {courseId}");

            return request;
        }

        public async Task<GradeRecheckRequest> UpdateRecheckRequestStatusAsync(string requestId, string status)
        {
            var request = await _context.GradeRecheckRequests
                .Include(r => r.Student)
                .FirstOrDefaultAsync(r => r.Id == requestId);

            if (request == null)
                throw new InvalidOperationException("Request not found");

            request.Status = status;
            request.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // Notify student about the status update
            await _emailService.SendEmailAsync(
                request.Student.Email,
                "Grade Recheck Request Update",
                $"Your grade recheck request for course {request.CourseId} has been {status}");

            return request;
        }

        public async Task<List<GradeRecheckRequest>> GetStudentRecheckRequestsAsync(string studentId)
        {
            return await _context.GradeRecheckRequests
                .Where(r => r.StudentId == studentId)
                .Include(r => r.Course)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<GradeRecheckRequest>> GetAllPendingRecheckRequestsAsync()
        {
            return await _context.GradeRecheckRequests
                .Where(r => r.Status == "Pending")
                .Include(r => r.Student)
                .Include(r => r.Course)
                .OrderBy(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<GradeNotification>> GetStudentGradeNotificationsAsync(string studentId)
        {
            return await _context.GradeNotifications
                .Where(n => n.StudentId == studentId)
                .Include(n => n.Course)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task MarkNotificationAsReadAsync(string notificationId)
        {
            var notification = await _context.GradeNotifications
                .FirstOrDefaultAsync(n => n.Id == notificationId);

            if (notification == null)
                throw new InvalidOperationException("Notification not found");

            notification.IsRead = true;
            await _context.SaveChangesAsync();
        }
    }
}