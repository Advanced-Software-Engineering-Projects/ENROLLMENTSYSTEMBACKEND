using System;
using System.Collections.Generic;

namespace ENROLLMENTSYSTEMBACKEND.Models.Temp;

public partial class GradeNotification
{
    public string Id { get; set; } = null!;

    public string StudentId { get; set; } = null!;

    public string CourseId { get; set; } = null!;

    public string OldGrade { get; set; } = null!;

    public string NewGrade { get; set; } = null!;

    public string NotificationType { get; set; } = null!;

    public bool IsRead { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
