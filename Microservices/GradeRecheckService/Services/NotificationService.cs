using System;

namespace GradeRecheckMicroservice.Services
{
    public class NotificationService
    {
        public void NotifyGradeChange(string studentId, string courseId, string message)
        {
            // Simulate sending notification (e.g., email or in-app notification)
            Console.WriteLine($"Notification sent to Student {studentId} for Course {courseId}: {message}");
        }
    }
}
