namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public interface IEmailService
    {
        Task SendFormNotificationAsync(string submissionId, string formType, string studentEmail);
    }
}
