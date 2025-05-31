namespace ENROLLMENTSYSTEMBACKEND.DTOs
{
    public class TimetableDto
    {
        public string StudentId { get; set; }
        public string CourseCode { get; set; }
        public string Semester { get; set; }
        public string Date { get; set; } // YYYY-MM-DD
        public string StartTime { get; set; } // HH:MM
        public string EndTime { get; set; } // HH:MM
    }
}