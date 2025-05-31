using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public interface IGradeService
    {
        Task<AcademicRecordsDto> GetAcademicRecordsAsync(string studentId);
        Task<TranscriptDto> GetTranscriptAsync(string studentId);
        Task<double> CalculateGPAAsync(string studentId);
        Task<ProgramAuditDto> GetProgramAuditAsync(string studentId);
        Task<EnrollmentStatusDto> GetEnrollmentStatusAsync(string studentId);
        Task<bool> CheckGraduationEligibilityAsync(string studentId);
        Task<GraduationApplicationDto> ApplyForGraduationAsync(string studentId);
        Task<GraduationApplicationDto> GetGraduationStatusAsync(string studentId);
        Task<List<Grade>> GetGradesByStudentIdAsync(string studentId);
        Task<FormSubmissionDto> UpdateGradeAsync(UpdateGradeDto updateGradeDto);
        Task<List<GpaTrendDto>> GetGpaTrendAsync(string studentId);
    }
}
