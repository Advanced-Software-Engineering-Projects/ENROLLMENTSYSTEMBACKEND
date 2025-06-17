using System;
using System.Collections.Generic;

namespace ENROLLMENTSYSTEMBACKEND.Models.Temp;

public partial class RegistrationPeriod
{
    public string RegistrationPeriodId { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public TimeSpan StartTime { get; set; }

    public TimeSpan EndTime { get; set; }

    public bool IsActive { get; set; }
}
