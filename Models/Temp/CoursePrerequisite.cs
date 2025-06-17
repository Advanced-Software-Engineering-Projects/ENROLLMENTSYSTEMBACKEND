using System;
using System.Collections.Generic;

namespace ENROLLMENTSYSTEMBACKEND.Models.Temp;

public partial class CoursePrerequisite
{
    public int Id { get; set; }

    public string CourseId { get; set; } = null!;

    public string PrerequisiteCourseId { get; set; } = null!;

    public virtual Course Course { get; set; } = null!;

    public virtual Course PrerequisiteCourse { get; set; } = null!;
}
