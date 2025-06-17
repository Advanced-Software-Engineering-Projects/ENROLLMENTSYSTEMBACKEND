using System;
using System.Collections.Generic;

namespace ENROLLMENTSYSTEMBACKEND.Models.Temp;

public partial class FeeHold
{
    public string Id { get; set; } = null!;

    public string StudentId { get; set; } = null!;

    public string Reason { get; set; } = null!;

    public DateTime DateApplied { get; set; }
}
