using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<IEnumerable<TimetableDto>> GetTimetablesByStudentIdAsync(string studentId, string semester)
        {
            var timetables = await _timetableRepository.GetTimetablesByStudentIdAndSemesterAsync(studentId, semester);
            if (timetables == null || !timetables.Any())
            {
                throw new InvalidOperationException("No timetable found for student");
            }

            return timetables.Select(t => new TimetableDto
            {
                StudentId = t.StudentId,
                CourseCode = t.CourseCode,
                Semester = t.Semester,
                Date = t.Date.ToString("yyyy-MM-dd"),
                StartTime = t.StartTime.ToString(@"hh\:mm"),
                EndTime = t.EndTime.ToString(@"hh\:mm")
            });
        }

        public async Task<TimetableDto> AddTimetableAsync(TimetableDto timetableDto)
        {
            // Convert DTO to model
            var timetable = new Timetable
            {
                StudentId = timetableDto.StudentId,
                CourseCode = timetableDto.CourseCode,
                Semester = timetableDto.Semester,
                Date = DateTime.Parse(timetableDto.Date),
                StartTime = TimeSpan.Parse(timetableDto.StartTime),
                EndTime = TimeSpan.Parse(timetableDto.EndTime)
            };

            var addedTimetable = await _timetableRepository.AddTimetableAsync(timetable);

            // Convert back to DTO
            return new TimetableDto
            {
                StudentId = addedTimetable.StudentId,
                CourseCode = addedTimetable.CourseCode,
                Semester = addedTimetable.Semester,
                Date = addedTimetable.Date.ToString("yyyy-MM-dd"),
                StartTime = addedTimetable.StartTime.ToString(@"hh\:mm"),
                EndTime = addedTimetable.EndTime.ToString(@"hh\:mm")
            };
        }
    }
}
