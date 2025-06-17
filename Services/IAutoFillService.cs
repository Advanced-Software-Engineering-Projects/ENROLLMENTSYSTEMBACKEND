using ENROLLMENTSYSTEMBACKEND.DTOs;
using System.Threading.Tasks;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public interface IAutoFillService
    {
        Task<FormAutoFillDataDto> GetAutoFillDataAsync(string studentId, string formType);
        Task<bool> ValidateAutoFillDataAsync(string studentId, FormAutoFillDataDto data);
        Task<Dictionary<string, string>> GetFormFieldMappingsAsync(string formType);
    }
} 