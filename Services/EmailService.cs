namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendFormNotificationAsync(string submissionId, string formType, string studentEmail)
        {
            // Placeholder for email sending logic
            Console.WriteLine($"Sending email to {studentEmail} for form {formType} with submission ID {submissionId}");
            await Task.CompletedTask;
        }
    }
}
