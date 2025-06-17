using System;
using System.Collections.Generic;

namespace ENROLLMENTSYSTEMBACKEND.Models.Temp;

public partial class Hold
{
    public string Id { get; set; } = null!;

    public string StudentId { get; set; } = null!;

    public string Service { get; set; } = null!;

    public string Reason { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual Student Student { get; set; } = null!;
}
