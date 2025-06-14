using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ENROLLMENTSYSTEMBACKEND.Microservices.FormsService.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string recipientId, string subject, string message);
        Task SendFormNotificationAsync(string studentId, string formType, string status);
    }

    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;

        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger;
        }

        public async Task SendEmailAsync(string recipientId, string subject, string message)
        {
            // In a real implementation, this would connect to an email service
            _logger.LogInformation($"Sending email to {recipientId} with subject '{subject}'");
            
            // Simulate async operation
            await Task.Delay(100);
        }

        public async Task SendFormNotificationAsync(string studentId, string formType, string status)
        {
            string subject = $"{formType} Form Status Update";
            string message = $"Your {formType} form has been {status.ToLower()}.";
            
            await SendEmailAsync(studentId, subject, message);
        }
    }
}