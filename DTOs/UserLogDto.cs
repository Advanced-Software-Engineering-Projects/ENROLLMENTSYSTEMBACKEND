namespace ENROLLMENTSYSTEMBACKEND.DTOs
{
    public class UserLogDto
    {
        public int UserLogId { get; set; }
        public required string EmailAddress { get; set; }
        public DateTime UserLogTimeStamp { get; set; }
        public required string UserLogActivity { get; set; }
        public required string UserProfileImagePath { get; set; }
    }
}