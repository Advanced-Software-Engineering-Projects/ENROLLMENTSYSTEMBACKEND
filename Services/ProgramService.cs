using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Models;
using ENROLLMENTSYSTEMBACKEND.Repositories;
using StudentSystemBackend.Repositories;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public class ProgramService : IProgramService
    {
        private readonly IStudentRepository _studentRepo;
        private readonly IProgramRepository _programRepo;

        public ProgramService(IStudentRepository studentRepo, IProgramRepository programRepo)
        {
            _studentRepo = studentRepo;
            _programRepo = programRepo;
        }

        public async Task<ProgramAuditDto> GetProgramAuditAsync(string studentId)
        {
            var student = await _studentRepo.GetByIdAsync(studentId);
            if (student == null) return null;

            var programVersion = await _programRepo.GetByIdAsync(student.ProgramVersionId);
            if (programVersion == null) return null;

            var programCourses = await _programRepo.GetProgramCoursesAsync(student.ProgramVersionId);
            var enrollments = await _studentRepo.GetEnrollmentsByStudentIdAsync(studentId);

            return new ProgramAuditDto
            {
                ProgramName = programVersion.Name,  // Corrected to access the Name property directly
                Years = new List<YearAuditDto>(),
                CompletionData = new List<CompletionDataDto>()
            };
        }

        public async Task GenerateAcademicPlanAsync(string studentId)
        {
            // Placeholder implementation - replace with actual logic as needed
            await Task.CompletedTask;
        }
    }
}