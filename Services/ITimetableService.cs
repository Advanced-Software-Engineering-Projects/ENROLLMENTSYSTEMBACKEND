using System.Collections.Generic;
using System.Threading.Tasks;
using ENROLLMENTSYSTEMBACKEND.DTOs;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public interface ITimetableService
    {
        Task<IEnumerable<TimetableDto>> GetTimetablesByStudentIdAsync(string studentId, string semester);
        Task<TimetableDto> AddTimetableAsync(TimetableDto timetableDto);
    }
}
