using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Models;
using ENROLLMENTSYSTEMBACKEND.Repositories;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public class StudentRecordService : IStudentRecordService
    {
        private readonly IStudentRecordRepository _studentRecordRepository;

        public StudentRecordService(IStudentRecordRepository studentRecordRepository)
        {
            _studentRecordRepository = studentRecordRepository;
        }

        public async Task<PaginatedStudentRecordsDto> GetStudentRecordsAsync(int page, int pageSize)
        {
            var (students, totalCount) = await _studentRecordRepository.GetStudentRecordsAsync(page, pageSize);
            
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            
            return new PaginatedStudentRecordsDto
            {
                Students = students.Select(MapToStudentRecordDto).ToList(),
                TotalCount = totalCount,
                TotalPages = totalPages,
                CurrentPage = page,
                PageSize = pageSize
            };
        }

        public async Task<StudentRecordDto> GetStudentByIdAsync(string studentId)
        {
            var student = await _studentRecordRepository.GetStudentByIdAsync(studentId);
            if (student == null)
            {
                return null;
            }
            
            return MapToStudentRecordDto(student);
        }

        private StudentRecordDto MapToStudentRecordDto(Student student)
        {
            return new StudentRecordDto
            {
                FirstName = student.FirstName,
                MiddleName = student.MiddleName,
                LastName = student.LastName,
                StudentId = student.StudentId,
                Dob = student.Dob.ToString("yyyy-MM-dd"),
                Email = student.Email,
                Phone = student.Phone,
                Avatar = student.AvatarUrl ?? "https://via.placeholder.com/150",
                Gender = student.Gender,
                Citizenship = student.Citizenship,
                Program = student.Program,
                StudentLevel = student.StudentLevel,
                StudentCampus = student.StudentCampus,
                ExamSite = student.ExamSite,
                MajorType = student.MajorType,
                Major1 = student.Major1,
                Major2 = student.Major2
            };
        }
    }
}