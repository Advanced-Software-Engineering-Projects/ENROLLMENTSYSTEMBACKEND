using System;

namespace ENROLLMENTSYSTEMBACKEND.Models
{
    public class Timetable
    {
        public int Id { get; set; }
        public string StudentId { get; set; }
        public string CourseCode { get; set; }
        public string Semester { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
