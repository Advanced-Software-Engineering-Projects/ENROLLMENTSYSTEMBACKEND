using System;
using System.Collections.Generic;

namespace ENROLLMENTSYSTEMBACKEND.Models.Temp;

public partial class GradeRecheckRequest
{
    public string Id { get; set; } = null!;

    public string StudentId { get; set; } = null!;

    public string CourseId { get; set; } = null!;

    public string CurrentGrade { get; set; } = null!;

    public string Reason { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Grade Grade { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
