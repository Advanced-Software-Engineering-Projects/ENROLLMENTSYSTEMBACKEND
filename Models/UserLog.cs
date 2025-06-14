namespace ENROLLMENTSYSTEMBACKEND.Models
{
    public class UserLog
    {
        public int UserLogId { get; set; }
        public string EmailAddress { get; set; }
        public DateTime UserLogTimeStamp { get; set; }
        public string UserLogActivity { get; set; }
        public string UserProfileImagePath { get; set; }
        
     
        public string UserId { get; set; }
        public User User { get; set; }
    }
}