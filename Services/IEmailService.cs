namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public interface IEmailService
    {
        Task<bool> SendGradeUpdateEmailAsync(string email, string courseCode, string newGrade);
        Task<bool> SendStatusUpdateEmailAsync(string email, string subject, string status);
        Task<bool> SendFormConfirmationEmailAsync(string email, string subject);
        Task<bool> SendFormNotificationAsync(string email, string formType, string status);
        Task<bool> SendEmailAsync(string email, string subject, string message);
    }
}