using System.Net;
using System.Net.Mail;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly IConfiguration _configuration;

        public EmailService(ILogger<EmailService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<bool> SendGradeUpdateEmailAsync(string email, string courseCode, string newGrade)
        {
            string subject = $"Grade Update for {courseCode}";
            string body = $"Dear Student,\n\nYour grade for {courseCode} has been updated to {newGrade}.\n\nRegards,\nUSP Administration";
            
            return await SendEmailAsync(email, subject, body);
        }

        public async Task<bool> SendStatusUpdateEmailAsync(string email, string subject, string status)
        {
            string body = $"Dear Student,\n\nYour application status has been updated to: {status}.\n\nRegards,\nUSP Administration";
            
            return await SendEmailAsync(email, subject, body);
        }

        public async Task<bool> SendFormConfirmationEmailAsync(string email, string subject)
        {
            string body = $"Dear Student,\n\nYour application has been received and is being processed.\n\nRegards,\nUSP Administration";
            
            return await SendEmailAsync(email, subject, body);
        }

        public async Task<bool> SendFormNotificationAsync(string email, string formType, string status)
        {
            string subject = $"{formType} Form Status Update";
            string body = $"Dear Student,\n\nYour {formType} form status has been updated to: {status}.\n\nRegards,\nUSP Administration";
            
            return await SendEmailAsync(email, subject, body);
        }

        public async Task<bool> SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                var smtpServer = _configuration["EmailSettings:SmtpServer"];
                var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
                var smtpUsername = _configuration["EmailSettings:Username"];
                var smtpPassword = _configuration["EmailSettings:Password"];
                var senderEmail = _configuration["EmailSettings:SenderEmail"];
                
                var client = new SmtpClient(smtpServer)
                {
                    Port = smtpPort,
                    Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = false
                };
                
                mailMessage.To.Add(to);
                
                await client.SendMailAsync(mailMessage);
                _logger.LogInformation($"Email sent successfully to {to}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send email to {to}");
                return false;
            }
        }
    }
}