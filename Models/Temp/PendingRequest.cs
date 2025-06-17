using System;
using System.Collections.Generic;

namespace ENROLLMENTSYSTEMBACKEND.Models.Temp;

public partial class PendingRequest
{
    public string Id { get; set; } = null!;

    public string StudentId { get; set; } = null!;

    public string CourseCode { get; set; } = null!;

    public string RequestType { get; set; } = null!;

    public DateTime Date { get; set; }

    public virtual Course CourseCodeNavigation { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
