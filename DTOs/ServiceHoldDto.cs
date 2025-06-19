namespace ENROLLMENTSYSTEMBACKEND.DTOs
{
    public class ServiceHoldDto
    {
        public string HoldId { get; set; }
        public string StudentId { get; set; }
        public string Service { get; set; }
        public string Reason { get; set; }
        public string CreatedAt { get; set; }
    }

    public class CreateServiceHoldDto
    {
        public string StudentId { get; set; }
        public int ServiceId { get; set; }
        public string Reason { get; set; }
    }

    public class StudentWithHoldsDto
    {
        public string StudentId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<ServiceHoldDto> Holds { get; set; } = new List<ServiceHoldDto>();
    }
}