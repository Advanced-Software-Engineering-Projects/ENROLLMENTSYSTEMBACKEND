using ENROLLMENTSYSTEMBACKEND.DTOs;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public interface IStudentService
    {
        Task<StudentDashboardDto> GetDashboardDataAsync(string studentId);
        Task<List<GradeDto>> GetGradesAsync(string studentId);
        Task<List<TimetableDto>> GetTimetableAsync(string studentId);
        Task<StudentProfileDto> GetProfileAsync(string studentId);
        Task<StudentProfileDto> UpdateProfileAsync(string studentId, StudentProfileDto profile);
        Task<AcademicRecordsDto> GetAcademicRecordsAsync(string studentId);
    }
}