using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Models;
using ENROLLMENTSYSTEMBACKEND.Repositories;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public class TimetableService : ITimetableService
    {
        private readonly ITimetableRepository _timetableRepository;

        public TimetableService(ITimetableRepository timetableRepository)
        {
            _timetableRepository = timetableRepository;
        }

        public async Task<List<Timetable>> GetTimetablesByStudentIdAsync(string studentId, string semester)
        {
            var timetables = await _timetableRepository.GetTimetablesByStudentIdAndSemesterAsync(studentId, semester);
            if (timetables == null || timetables.Count == 0)
            {
                throw new InvalidOperationException("No timetable found for student");
            }
            return timetables;
        }

        public async Task<Timetable> AddTimetableAsync(TimetableDto timetableDto)
        {
            var timetable = new Timetable
            {
                StudentId = timetableDto.StudentId,
                CourseCode = timetableDto.CourseCode,
                Semester = timetableDto.Semester,
                Date = DateTime.Parse(timetableDto.Date),
                StartTime = TimeSpan.Parse(timetableDto.StartTime),
                EndTime = TimeSpan.Parse(timetableDto.EndTime)
            };

            await _timetableRepository.AddTimetableAsync(timetable);
            return timetable;
        }
    }
}
