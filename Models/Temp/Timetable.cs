using System;
using System.Collections.Generic;

namespace ENROLLMENTSYSTEMBACKEND.Models.Temp;

public partial class Timetable
{
    public string StudentId { get; set; } = null!;

    public string CourseCode { get; set; } = null!;

    public string Semester { get; set; } = null!;

    public int Id { get; set; }

    public DateTime Date { get; set; }

    public TimeSpan StartTime { get; set; }

    public TimeSpan EndTime { get; set; }
}
