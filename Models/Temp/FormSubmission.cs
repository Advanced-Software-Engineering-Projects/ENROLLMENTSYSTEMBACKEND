using System;
using System.Collections.Generic;

namespace ENROLLMENTSYSTEMBACKEND.Models.Temp;

public partial class FormSubmission
{
    public string SubmissionId { get; set; } = null!;

    public string StudentId { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Telephone { get; set; } = null!;

    public string PostalAddress { get; set; } = null!;

    public string FormType { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string EmailStatus { get; set; } = null!;

    public DateTime SubmissionDate { get; set; }

    public string? FormData { get; set; }

    public string? SupportingDocuments { get; set; }

    public string? DateOfBirth { get; set; }

    public string? Sponsorship { get; set; }

    public string? CourseCode { get; set; }

    public string? CourseLecturer { get; set; }

    public string? CourseTitle { get; set; }

    public string? ReceiptNo { get; set; }

    public string? PaymentConfirmation { get; set; }

    public string? CurrentGrade { get; set; }

    public string? Campus { get; set; }

    public string? Semester { get; set; }

    public string? Year { get; set; }

    public string? Reason { get; set; }

    public string? ApplicantSignature { get; set; }

    public string? Date { get; set; }

    public string? Programme { get; set; }

    public string? ExamDate { get; set; }

    public string? ExamStartTime { get; set; }

    public string? ApplyingFor { get; set; }

    public string? NewGrade { get; set; }

    public string? MissedExams { get; set; }

    public string? DeclarationAgreed { get; set; }

    public string? MedicalDocumentation { get; set; }

    public string? Comments { get; set; }

    public string? CourseId { get; set; }

    public bool? IsCompleted { get; set; }

    public string? ProgramCode { get; set; }

    public string? ProgramId { get; set; }

    public virtual Course? Course { get; set; }

    public virtual Program? Program { get; set; }
}
