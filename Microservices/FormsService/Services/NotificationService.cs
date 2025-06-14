using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ENROLLMENTSYSTEMBACKEND.Microservices.FormsService.Services
{
    public interface INotificationService
    {
        Task NotifyFormApplicationStatusAsync(string studentId, string formType, string status);
    }

    public class NotificationService : INotificationService
    {
        private readonly ILogger<NotificationService> _logger;
        private readonly IEmailService _emailService;

        public NotificationService(
            ILogger<NotificationService> logger,
            IEmailService emailService)
        {
            _logger = logger;
            _emailService = emailService;
        }

        public async Task NotifyFormApplicationStatusAsync(string studentId, string formType, string status)
        {
            _logger.LogInformation($"Notifying student {studentId} of form {formType} application status: {status}");
            
            string subject = $"{formType} Form Status Update";
            string message = $"Your {formType} form has been {status.ToLower()}.";
            
            if (status.ToLower() == "submitted")
            {
                message += " We will review your application and notify you of any updates.";
            }
            else if (status.ToLower() == "approved")
            {
                message += " Your application has been approved.";
                
                if (formType == "CompletionProgramme")
                {
                    message += " Congratulations on completing your programme!";
                }
                else if (formType == "CompassionateAegrotat")
                {
                    message += " Please check your academic record for updates.";
                }
                else if (formType == "Reconsideration")
                {
                    message += " Your grade has been updated.";
                }
            }
            else if (status.ToLower() == "rejected")
            {
                message += " Please contact the academic office for more information.";
            }
            
            await _emailService.SendEmailAsync(studentId, subject, message);
        }
    }
}