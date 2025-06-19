using ENROLLMENTSYSTEMBACKEND.Models;
using ENROLLMENTSYSTEMBACKEND.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public class TimetableRepository : ITimetableRepository
    {
        private readonly EnrollmentInformationDbContext _context;

        public TimetableRepository(EnrollmentInformationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Timetable>> GetTimetablesByStudentIdAndSemesterAsync(string studentId, string semester)
        {
            return await _context.Timetables
                .Where(t => t.StudentId == studentId && t.Semester == semester)
                .ToListAsync();
        }

        public async Task<Timetable> AddTimetableAsync(Timetable timetable)
        {
            _context.Timetables.Add(timetable);
            await _context.SaveChangesAsync();
            return timetable;
        }

        public async Task UpdateTimetableAsync(Timetable timetable)
        {
            _context.Timetables.Update(timetable);
            await _context.SaveChangesAsync();
        }
    }
}