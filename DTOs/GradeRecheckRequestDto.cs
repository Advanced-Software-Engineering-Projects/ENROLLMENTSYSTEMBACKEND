namespace ENROLLMENTSYSTEMBACKEND.DTOs
{
    public class GradeRecheckRequestDto
    {
        public required string CourseId { get; set; }
        public required string Reason { get; set; }
    }

    public class GradeRecheckResponseDto
    {
        public required string Id { get; set; }
        public required string StudentId { get; set; }
        public required string CourseId { get; set; }
        public required string CourseName { get; set; }
        public required string CurrentGrade { get; set; }
        public required string Status { get; set; }
        public required string Reason { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}