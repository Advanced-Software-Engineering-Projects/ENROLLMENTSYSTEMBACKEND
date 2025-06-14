using ENROLLMENTSYSTEMBACKEND.DTOs;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public interface IStudentRecordService
    {
        Task<PaginatedStudentRecordsDto> GetStudentRecordsAsync(int page, int pageSize);
        Task<StudentRecordDto> GetStudentByIdAsync(string studentId);
    }
}