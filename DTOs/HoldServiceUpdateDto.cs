namespace ENROLLMENTSYSTEMBACKEND.DTOs
{
    public class HoldServiceUpdateDto
    {
        public required string StudentId { get; set; }
        public required List<string> RestrictedServices { get; set; }
    }

    public class StudentHoldStatusDto
    {
        public required string StudentId { get; set; }
        public required List<string> RestrictedServices { get; set; }
        public required List<string> AvailableServices { get; set; }
        public required List<HoldResponseDto> ActiveHolds { get; set; }
    }
}