using System.Threading.Tasks;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public interface ITranscriptService
    {
        Task<byte[]> GenerateTranscriptPdfAsync(string studentId);
        Task<string> GetStudentGpaAsync(string studentId);
        Task<bool> ValidateTranscriptRequestAsync(string studentId);
    }
}