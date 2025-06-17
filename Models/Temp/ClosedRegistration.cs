using System;
using System.Collections.Generic;

namespace ENROLLMENTSYSTEMBACKEND.Models.Temp;

public partial class ClosedRegistration
{
    public string Id { get; set; } = null!;

    public DateTime ClosedAt { get; set; }
}
