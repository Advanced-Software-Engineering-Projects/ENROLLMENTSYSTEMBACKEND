using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public interface ITimetableRepository
    {
        Task<List<Timetable>> GetTimetablesByStudentIdAndSemesterAsync(string studentId, string semester);
        Task AddTimetableAsync(Timetable timetable);
    }
}
