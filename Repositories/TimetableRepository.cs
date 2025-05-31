using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public class TimetableRepository : ITimetableRepository
    {
        private readonly List<Timetable> _timetables = new List<Timetable>();

        public async Task<List<Timetable>> GetTimetablesByStudentIdAndSemesterAsync(string studentId, string semester)
        {
            return await Task.FromResult(_timetables.Where(t => t.StudentId == studentId && t.Semester == semester).ToList());
        }

        public async Task AddTimetableAsync(Timetable timetable)
        {
            timetable.Id = Guid.NewGuid().ToString();
            _timetables.Add(timetable);
            await Task.CompletedTask;
        }
    }
}
