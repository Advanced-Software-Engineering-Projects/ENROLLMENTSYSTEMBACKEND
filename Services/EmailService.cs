using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendFormNotificationAsync(string submissionId, string formType, string studentEmail)
        {
            var smtpHost = _configuration["EmailSettings:SmtpHost"];
            var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
            var smtpUser = _configuration["EmailSettings:SmtpUser"];
            var smtpPass = _configuration["EmailSettings:SmtpPass"];
            var fromEmail = _configuration["EmailSettings:FromEmail"];

            using (var client = new SmtpClient(smtpHost, smtpPort))
            {
                client.Credentials = new NetworkCredential(smtpUser, smtpPass);
                client.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail),
                    Subject = $"Notification: {formType}",
                    Body = $"Dear Student,\n\nThis is a notification regarding your form submission with ID {submissionId} for {formType}.\n\nBest regards,\nUniversity Administration",
                    IsBodyHtml = false,
                };
                mailMessage.To.Add(studentEmail);

                try
                {
                    await client.SendMailAsync(mailMessage);
                    _logger.LogInformation($"Email sent to {studentEmail} with subject 'Notification: {formType}'.");
                }
                catch (System.Exception ex)
                {
                    _logger.LogError(ex, $"Failed to send email to {studentEmail}.");
                    throw;
                }
            }
        }
    }
}