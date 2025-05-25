namespace ENROLLMENTSYSTEMBACKEND.Models
{
    public class RegistrationPeriod
    {
        public int RegistrationPeriodId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}
