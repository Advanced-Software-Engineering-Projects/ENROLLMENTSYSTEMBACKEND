using System;
using System.Collections.Generic;

namespace ENROLLMENTSYSTEMBACKEND.Models.Temp;

public partial class Fee
{
    public string Id { get; set; } = null!;

    public string StudentId { get; set; } = null!;

    public decimal Amount { get; set; }

    public string Semester { get; set; } = null!;

    public DateTime DueDate { get; set; }

    public bool IsPaid { get; set; }
}
