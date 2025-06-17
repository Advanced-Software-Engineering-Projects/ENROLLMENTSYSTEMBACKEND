using System;
using System.Collections.Generic;

namespace ENROLLMENTSYSTEMBACKEND.Models.Temp;

public partial class Grade
{
    public string StudentId { get; set; } = null!;

    public string CourseId { get; set; } = null!;

    public string GradeValue { get; set; } = null!;

    public virtual Course Course { get; set; } = null!;

    public virtual ICollection<GradeRecheckRequest> GradeRecheckRequests { get; } = new List<GradeRecheckRequest>();

    public virtual Student Student { get; set; } = null!;
}
