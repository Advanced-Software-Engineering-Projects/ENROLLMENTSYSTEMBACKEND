using System;
using System.Collections.Generic;

namespace ENROLLMENTSYSTEMBACKEND.Models.Temp;

public partial class Course
{
    public string CourseId { get; set; } = null!;

    public string CourseCode { get; set; } = null!;

    public string CourseName { get; set; } = null!;

    public int Credits { get; set; }

    public string Description { get; set; } = null!;

    public string Program { get; set; } = null!;

    public int Year { get; set; }

    public DateTime DueDate { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<CoursePrerequisite> CoursePrerequisiteCourses { get; } = new List<CoursePrerequisite>();

    public virtual ICollection<CoursePrerequisite> CoursePrerequisitePrerequisiteCourses { get; } = new List<CoursePrerequisite>();

    public virtual ICollection<Enrollment> Enrollments { get; } = new List<Enrollment>();

    public virtual ICollection<FormSubmission> FormSubmissions { get; } = new List<FormSubmission>();

    public virtual ICollection<GradeNotification> GradeNotifications { get; } = new List<GradeNotification>();

    public virtual ICollection<GradeRecheckRequest> GradeRecheckRequests { get; } = new List<GradeRecheckRequest>();

    public virtual ICollection<Grade> Grades { get; } = new List<Grade>();

    public virtual ICollection<PendingRequest> PendingRequests { get; } = new List<PendingRequest>();
}
