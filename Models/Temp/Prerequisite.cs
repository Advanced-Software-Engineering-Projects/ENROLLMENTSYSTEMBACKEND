using System;
using System.Collections.Generic;

namespace ENROLLMENTSYSTEMBACKEND.Models.Temp;

public partial class Prerequisite
{
    public string CourseCode { get; set; } = null!;

    public string PrerequisiteCourseCode { get; set; } = null!;
}
