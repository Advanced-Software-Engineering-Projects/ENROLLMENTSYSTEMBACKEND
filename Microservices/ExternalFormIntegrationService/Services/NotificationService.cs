using System;

namespace ExternalFormIntegrationService.Services
{
    public class NotificationService
    {
        public void SendNotification(string studentId, string message)
        {
            // Simulate sending notification (e.g., in-app notification)
            Console.WriteLine($"Notification sent to Student {studentId}: {message}");
        }

        public void SendEmail(string studentId, string subject, string body)
        {
            // Simulate sending email
            Console.WriteLine($"Email sent to Student {studentId} - Subject: {subject} - Body: {body}");
        }
    }
}
