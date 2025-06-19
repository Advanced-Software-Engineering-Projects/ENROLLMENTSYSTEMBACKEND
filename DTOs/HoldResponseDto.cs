namespace ENROLLMENTSYSTEMBACKEND.DTOs
{
    public class HoldResponseDto
    {
        public required string Id { get; set; }
        public required string StudentId { get; set; }
        public required string Service { get; set; }
        public required string Reason { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
