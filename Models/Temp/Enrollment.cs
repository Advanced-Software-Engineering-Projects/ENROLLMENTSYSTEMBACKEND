using System;
using System.Collections.Generic;

namespace ENROLLMENTSYSTEMBACKEND.Models.Temp;

public partial class Enrollment
{
    public string StudentId { get; set; } = null!;

    public string CourseId { get; set; } = null!;

    public string? EnrollmentId { get; set; }

    public string CourseCode { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime EnrollmentDate { get; set; }

    public DateTime? CompletionDate { get; set; }

    public string Grade { get; set; } = null!;

    public string Semester { get; set; } = null!;

    public int Year { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
