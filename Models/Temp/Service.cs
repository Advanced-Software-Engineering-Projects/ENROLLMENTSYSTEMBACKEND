using System;
using System.Collections.Generic;

namespace ENROLLMENTSYSTEMBACKEND.Models.Temp;

public partial class Service
{
    public int ServicesId { get; set; }

    public string ServiceName { get; set; } = null!;

    public string ServiceDescription { get; set; } = null!;

    public virtual ICollection<ServiceHold> ServiceHolds { get; } = new List<ServiceHold>();
}
