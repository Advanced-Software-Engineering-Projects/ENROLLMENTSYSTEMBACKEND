using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ENROLLMENTSYSTEMBACKEND.Data;
using ENROLLMENTSYSTEMBACKEND.Models;
using Microsoft.EntityFrameworkCore;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public class TimetableRepository : ITimetableRepository
    {
        private readonly EnrollmentInfromation _context;

        public TimetableRepository(EnrollmentInfromation context)
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
    }
}
