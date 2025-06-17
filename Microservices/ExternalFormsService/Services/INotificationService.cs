using System.Threading.Tasks;

namespace ExternalFormsService.Services
{
    public interface INotificationService
    {
        Task SendNotificationAsync(string userId, string message, string type);
        Task SendBulkNotificationAsync(string[] userIds, string message, string type);
    }
} 