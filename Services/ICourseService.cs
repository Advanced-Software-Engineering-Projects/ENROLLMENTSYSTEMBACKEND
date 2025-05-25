using ENROLLMENTSYSTEMBACKEND.DTOs;

    public interface ICourseService
    {
        Task<List<CourseDto>> GetAvailableCoursesAsync(string studentId, string semester);
        Task EnrollCourseAsync(string studentId, int courseId, string semester);
        Task DropCourseAsync(string studentId, int courseId, string semester);
        Task<List<PrerequisiteDto>> GetPrerequisitesAsync(int courseId);
        Task<PrerequisiteGraphDto> GetPrerequisiteGraphAsync();
    }
