using System;
using System.Collections.Generic;

namespace ENROLLMENTSYSTEMBACKEND.Models.Temp;

public partial class ServiceHold
{
    public string StudentId { get; set; } = null!;

    public int ServiceId { get; set; }

    public string HoldId { get; set; } = null!;

    public string Reason { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual Service Service { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
