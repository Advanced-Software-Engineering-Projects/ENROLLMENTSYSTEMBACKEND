using System.Collections.Generic;
using System.Threading.Tasks;
using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public interface ITimetableRepository
    {
        Task<IEnumerable<Timetable>> GetTimetablesByStudentIdAndSemesterAsync(string studentId, string semester);
        Task<Timetable> AddTimetableAsync(Timetable timetable);
    }
}
