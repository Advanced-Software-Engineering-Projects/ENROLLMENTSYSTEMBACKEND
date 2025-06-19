using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public interface INotificationService
    {
        Task NotifyGradeChangeAsync(string studentId, string courseId, string newGrade);
        Task NotifyFormApplicationStatusAsync(string studentId, string formType, string status);
    }

    public class NotificationService : INotificationService
    {
        private readonly ILogger<NotificationService> _logger;
        private readonly IEmailService _emailService;

        public NotificationService(ILogger<NotificationService> logger, IEmailService emailService)
        {
            _logger = logger;
            _emailService = emailService;
        }

        public async Task NotifyGradeChangeAsync(string studentId, string courseId, string newGrade)
        {
            _logger.LogInformation($"Notifying student {studentId} of grade change in course {courseId} to {newGrade}.");
            await _emailService.SendFormNotificationAsync(studentId, courseId, newGrade).ConfigureAwait(false);
        }

        public async Task NotifyFormApplicationStatusAsync(string studentId, string formType, string status)
        {
            _logger.LogInformation($"Notifying student {studentId} of form {formType} application status: {status}.");
            await _emailService.SendFormNotificationAsync(studentId, formType, status).ConfigureAwait(false);
        }
    }
}
