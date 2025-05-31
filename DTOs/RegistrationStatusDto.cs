namespace ENROLLMENTSYSTEMBACKEND.DTOs
{
    public class RegistrationStatusDto
    {
        public bool IsOpen { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
