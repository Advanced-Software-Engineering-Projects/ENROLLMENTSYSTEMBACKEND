using System.Collections.Generic;

namespace ENROLLMENTSYSTEMBACKEND.DTOs
{
    public class FormSubmissionsResponseDto
    {
        public List<FormSubmissionDto> Forms { get; set; } = new List<FormSubmissionDto>();
        public List<FormSubmissionDto>? Reconsideration { get; set; }
        public List<FormSubmissionDto>? CompassionateAegrotat { get; set; }
        public List<FormSubmissionDto>? CompletionProgramme { get; set; }
    }
}