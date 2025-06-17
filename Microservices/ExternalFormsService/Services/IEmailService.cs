using System.Threading.Tasks;

namespace ExternalFormsService.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body, bool isHtml = false);
        Task SendBulkEmailAsync(string[] to, string subject, string body, bool isHtml = false);
    }
} 