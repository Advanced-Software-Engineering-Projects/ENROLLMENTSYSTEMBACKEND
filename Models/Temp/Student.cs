using System;
using System.Collections.Generic;

namespace ENROLLMENTSYSTEMBACKEND.Models.Temp;

public partial class Student
{
    public string Id { get; set; } = null!;

    public string StudentId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string MiddleName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Program { get; set; } = null!;

    public DateTime Dob { get; set; }

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Avatar { get; set; } = null!;

    public string AvatarUrl { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public string Citizenship { get; set; } = null!;

    public string StudentLevel { get; set; } = null!;

    public string StudentCampus { get; set; } = null!;

    public string ExamSite { get; set; } = null!;

    public string MajorType { get; set; } = null!;

    public string Major1 { get; set; } = null!;

    public string Major2 { get; set; } = null!;

    public string ProgramId { get; set; } = null!;

    public virtual ICollection<Enrollment> Enrollments { get; } = new List<Enrollment>();

    public virtual ICollection<GradeNotification> GradeNotifications { get; } = new List<GradeNotification>();

    public virtual ICollection<GradeRecheckRequest> GradeRecheckRequests { get; } = new List<GradeRecheckRequest>();

    public virtual ICollection<Grade> Grades { get; } = new List<Grade>();

    public virtual ICollection<Hold> Holds { get; } = new List<Hold>();

    public virtual ICollection<PendingRequest> PendingRequests { get; } = new List<PendingRequest>();

    public virtual ICollection<ServiceHold> ServiceHolds { get; } = new List<ServiceHold>();
}
