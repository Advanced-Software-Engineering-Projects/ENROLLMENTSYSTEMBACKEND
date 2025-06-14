using System;

namespace ENROLLMENTSYSTEMBACKEND.Models
{
    public class FormSubmission
    {
        public string SubmissionId { get; set; } = string.Empty;
        public string StudentId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;
        public string PostalAddress { get; set; } = string.Empty;
        public string FormType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string EmailStatus { get; set; } = string.Empty;
        public DateTime SubmissionDate { get; set; }

        // Additional form-specific properties
        public string? FormData { get; set; }
        public string? SupportingDocuments { get; set; }
        
        // Properties needed by forms
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
    }
}