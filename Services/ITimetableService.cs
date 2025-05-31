using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public interface ITimetableService
    {
        Task<List<Timetable>> GetTimetablesByStudentIdAsync(string studentId, string semester);
        Task<Timetable> AddTimetableAsync(TimetableDto timetableDto);
    }
}
