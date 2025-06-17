using System;
using System.Collections.Generic;

namespace ENROLLMENTSYSTEMBACKEND.Models.Temp;

public partial class PaymentRecord
{
    public string StudentId { get; set; } = null!;

    public string FeeId { get; set; } = null!;

    public string? Id { get; set; }

    public decimal AmountPaid { get; set; }

    public DateTime PaymentDate { get; set; }

    public string Semester { get; set; } = null!;
}
