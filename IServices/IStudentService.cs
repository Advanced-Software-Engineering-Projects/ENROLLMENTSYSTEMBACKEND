using ENROLLMENTSYSTEMBACKEND.DTOs;

namespace ENROLLMENTSYSTEMBACKEND.IServices
{
    public interface IStudentService
    {
        Task<StudentDto> AuthenticateStudentAsync(string studentId, string password);
        Task<StudentDto> LoginAsync(string studentId, string password);
        Task<StudentDto> RegisterAsync(StudentDto studentDto);
        Task<StudentDto> UpdateProfileAsync(string studentId, StudentDto studentDto);
        Task<IEnumerable<CourseDto>> GetAvailableCoursesAsync(string studentId, string semester);
        Task<IEnumerable<CourseDto>> SearchCoursesAsync(string studentId, string? keyword, string? semester, int? creditsMin, int? creditsMax);
        Task<bool> RegisterCourseAsync(string studentId, int courseId, string semester);
        Task<bool> DropCourseAsync(string studentId, int enrollmentId);
        Task<IEnumerable<EnrollmentDto>> GetTimetableAsync(string studentId, string semester);
        Task<IEnumerable<EnrollmentDto>> GetAcademicRecordsAsync(string studentId);
        Task<IEnumerable<FeeDto>> GetFeeInformationAsync(string studentId);
        Task<bool> MarkFeeAsPaidAsync(string studentId, int feeId);
        Task<bool> HasFeeHoldsAsync(string studentId);
        Task<DegreeProgressDto> GetDegreeProgressAsync(string studentId);
        Task<IEnumerable<PrerequisiteNode>> GetPrerequisiteGraphAsync(string studentId, int courseId);
        Task<ProgramVersionDto> GetStudentProgramAsync(string studentId);
        Task<int> GetCurrentlyEnrolledCoursesAsync(string studentId, string semester);
        Task<int> GetCoursesCompletedInCurrentYearAsync(string studentId);
        Task<int> GetTotalCoursesCompletedAsync(string studentId);
        Task<double> GetEnrollmentProgressAsync(string studentId);
        Task<Dictionary<string, double>> GetGpaBySemesterAsync(string studentId);
    }
}