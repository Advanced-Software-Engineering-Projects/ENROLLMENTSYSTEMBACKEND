using ENROLLMENTSYSTEMBACKEND.DTOs;

namespace ENROLLMENTSYSTEMBACKEND.Microservices.FormsService.Services
{
    public interface IStudentFormsService
    {
        Task<FormSubmissionDto> SubmitReconsiderationFormAsync(ReconsiderationFormDto formDto);
        Task<FormSubmissionDto> SubmitCompassionateAegrotatFormAsync(CompassionateAegrotatFormDto formDto);
        Task<FormSubmissionDto> SubmitCompletionProgrammeFormAsync(CompletionProgrammeFormDto formDto);
        Task<FormSubmissionsResponseDto> GetStudentFormsAsync(string studentId);
        Task<bool> CheckEligibilityAsync(string studentId, string formType);
    }
}