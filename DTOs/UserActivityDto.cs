namespace ENROLLMENTSYSTEMBACKEND.DTOs
{
    public class UserActivityDto
    {
        public int ActivityId { get; set; }
        public string UserId { get; set; }
        public string UserType { get; set; }
        public string Action { get; set; }
        public DateTime Timestamp { get; set; }
        public string Details { get; set; }
    }
}
