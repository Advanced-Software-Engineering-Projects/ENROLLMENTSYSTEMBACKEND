using System.Threading.Tasks;
using System.Collections.Generic;
using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public interface IGradeRecheckService
    {
        Task<GradeRecheckRequest> CreateRecheckRequestAsync(string studentId, string courseId, string reason);
        Task<GradeRecheckRequest> UpdateRecheckRequestStatusAsync(string requestId, string status);
        Task<List<GradeRecheckRequest>> GetStudentRecheckRequestsAsync(string studentId);
        Task<List<GradeRecheckRequest>> GetAllPendingRecheckRequestsAsync();
        Task<List<GradeNotification>> GetStudentGradeNotificationsAsync(string studentId);
        Task MarkNotificationAsReadAsync(string notificationId);
    }
}