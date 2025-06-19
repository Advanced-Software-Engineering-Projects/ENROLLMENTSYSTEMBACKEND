namespace ENROLLMENTSYSTEMBACKEND.DTOs
{
    public class FormSubmissionDto
    {
        public int Id { get; set; }
        public int SubmissionId { get; set; }
        public string StudentId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;
        public string PostalAddress { get; set; } = string.Empty;
        public string FormType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string EmailStatus { get; set; } = string.Empty;
        public DateTime SubmissionDate { get; set; }

        // Properties for all form types to fix errors in FormConfigurationService and FormService
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
        public string? SupportingDocuments { get; set; }
        public string? ApplicantSignature { get; set; }
        public string? Date { get; set; }
        public string? Programme { get; set; }
        public string? ExamDate { get; set; }
        public string? ExamStartTime { get; set; }
        public string? ApplyingFor { get; set; }
        public string? NewGrade { get; set; }
        
        // Collections for form submissions
        public List<ReconsiderationFormDto>? Reconsideration { get; set; }
        public List<CompassionateAegrotatFormDto>? CompassionateAegrotat { get; set; }
        public List<CompletionProgrammeFormDto>? CompletionProgramme { get; set; }
    }

    public class ReconsiderationFormDto : FormSubmissionDto
    {
        public new string DateOfBirth { get; set; } = string.Empty;
        public new string Sponsorship { get; set; } = string.Empty;
        public new string CourseCode { get; set; } = string.Empty;
        public new string CourseLecturer { get; set; } = string.Empty;
        public new string CourseTitle { get; set; } = string.Empty;
        public new string ReceiptNo { get; set; } = string.Empty;
        public new string PaymentConfirmation { get; set; } = string.Empty;
        public new string CurrentGrade { get; set; } = string.Empty;
    }

    public class CompassionateAegrotatFormDto : FormSubmissionDto
    {
        public new string Campus { get; set; } = string.Empty;
        public new string Semester { get; set; } = string.Empty;
        public new string Year { get; set; } = string.Empty;
        public List<MissedExamDto> MissedExams { get; set; } = new List<MissedExamDto>();
        public new string Reason { get; set; } = string.Empty;
        public new string SupportingDocuments { get; set; } = string.Empty;
        public new string ApplicantSignature { get; set; } = string.Empty;
        public new string Date { get; set; } = string.Empty;
    }

    public class CompletionProgrammeFormDto : FormSubmissionDto
    {
        public new string DateOfBirth { get; set; } = string.Empty;
        public new string Programme { get; set; } = string.Empty;
        public bool DeclarationAgreed { get; set; }
        public new string ApplicantSignature { get; set; } = string.Empty;
        public new string Date { get; set; } = string.Empty;
    }

    public class MissedExamDto
    {
        public string CourseCode { get; set; } = string.Empty;
        public string ExamDate { get; set; } = string.Empty;
        public string ExamStartTime { get; set; } = string.Empty;
        public string ApplyingFor { get; set; } = string.Empty;
    }

    public class GradeUpdateDto
    {
        public int SubmissionId { get; set; }
        public string NewGrade { get; set; } = string.Empty;
    }

    public class StatusUpdateDto
    {
        public int SubmissionId { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class EmailResendDto
    {
        public int SubmissionId { get; set; }
        public string FormType { get; set; } = string.Empty;
        public string? StudentEmail { get; set; }
    } 
}