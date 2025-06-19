using ENROLLMENTSYSTEMBACKEND.Data;
using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Models;
using ENROLLMENTSYSTEMBACKEND.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public class CourseManagementService : ICourseManagementService
    {
        private readonly ICourseManagementRepository _courseManagementRepository;
        private readonly EnrollmentInformationDbContext _context;

        public CourseManagementService(ICourseManagementRepository courseManagementRepository, EnrollmentInformationDbContext context)
        {
            _courseManagementRepository = courseManagementRepository;
            _context = context;
        }

        public async Task<CourseRegistrationStatusDto> GetRegistrationStatusAsync()
        {
            var openRegistrations = await _courseManagementRepository.GetOpenRegistrationsAsync();
            var closedRegistrations = await _courseManagementRepository.GetClosedRegistrationsAsync();

            var result = new CourseRegistrationStatusDto
            {
                OpenRegistrations = openRegistrations.Select(r => new CourseRegistrationPeriodDto
                {
                    Courses = r.CourseCodes,
                    StartDate = r.StartDate.ToString("yyyy-MM-dd"),
                    StartTime = r.StartTime.ToString(@"h\:mm"),
                    EndDate = r.EndDate.ToString("yyyy-MM-dd"),
                    EndTime = r.EndTime.ToString(@"h\:mm")
                }).ToList(),

                ClosedRegistrations = closedRegistrations.Select(r => new ClosedRegistrationDto
                {
                    Courses = r.CourseCodes,
                    ClosedAt = r.ClosedAt.ToString("yyyy-MM-dd h:mm tt")
                }).ToList()
            };

            return result;
        }

        public async Task OpenCourseRegistrationAsync(CourseManagementDto request)
        {
            if (request == null || request.CourseCodes == null || !request.CourseCodes.Any())
            {
                throw new ArgumentException("Course codes are required");
            }

            if (string.IsNullOrEmpty(request.StartDate) || string.IsNullOrEmpty(request.EndDate) ||
                string.IsNullOrEmpty(request.StartTime) || string.IsNullOrEmpty(request.EndTime))
            {
                throw new ArgumentException("Start and end dates/times are required");
            }

            // Parse dates and times
            if (!DateTime.TryParse(request.StartDate, out DateTime startDate) ||
                !DateTime.TryParse(request.EndDate, out DateTime endDate))
            {
                throw new ArgumentException("Invalid date format");
            }

            if (!TimeSpan.TryParse(request.StartTime, out TimeSpan startTime) ||
                !TimeSpan.TryParse(request.EndTime, out TimeSpan endTime))
            {
                throw new ArgumentException("Invalid time format");
            }

            // Validate dates
            if (endDate < startDate || (endDate == startDate && endTime <= startTime))
            {
                throw new ArgumentException("End date/time must be after start date/time");
            }

            if (endDate < DateTime.Today || (endDate == DateTime.Today && endTime < TimeSpan.FromHours(DateTime.Now.Hour)))
            {
                throw new ArgumentException("End date/time cannot be in the past");
            }

            if (_context == null)
            {
                throw new InvalidOperationException("Database context is not initialized.");
            }

            // Update IsActive for matching courses
            var coursesToUpdate = await _context.Courses
                .Where(c => request.CourseCodes.Contains(c.CourseCode))
                .ToListAsync();

            //if (coursesToUpdate.Count != request.CourseCodes.Count)
            //{
            //    var missingCodes = request.CourseCodes.Except(coursesToUpdate.Select(c => c.CourseCode)).ToList();
            //    throw new ArgumentException($"The following course codes were not found: {string.Join(", ", missingCodes)}");
            //}

            foreach (var course in coursesToUpdate)
            {
                course.IsActive = true;
            }

            // Create registration period
            var registrationPeriod = new CourseRegistrationPeriod
            {
                CourseCodes = request.CourseCodes,
                StartDate = startDate,
                EndDate = endDate,
                StartTime = startTime,
                EndTime = endTime,
                IsActive = true
            };

            await _courseManagementRepository.AddOpenRegistrationAsync(registrationPeriod);

            // Save changes to courses and registration period
            await _context.SaveChangesAsync();
        }

        public async Task CloseCourseRegistrationAsync(CloseCourseRegistrationDto request)
        {
            if (request == null || !request.CourseCodes.Any())
            {
                throw new ArgumentException("Course codes are required");
            }

            await _courseManagementRepository.CloseRegistrationAsync(request.CourseCodes);
        }

        public async Task<RegistrationMetricsDto> GetRegistrationMetricsAsync()
        {
            var totalRegistrations = await _courseManagementRepository.GetTotalRegistrationsCountAsync();
            var droppedCourses = await _courseManagementRepository.GetTotalDroppedCoursesCountAsync();

            return new RegistrationMetricsDto
            {
                TotalRegistrations = totalRegistrations,
                DroppedCourses = droppedCourses
            };
        }

        public async Task<List<CourseDto>> GetAllCoursesAsync()
        {
            var courses = await _courseManagementRepository.GetAllCoursesAsync();
            
            return courses.Select(c => new CourseDto
            {
                CourseId = c.CourseId,
                CourseCode = c.CourseCode,
                CourseName = c.CourseName,
                Program = c.Program,
                Year = c.Year
            }).ToList();
        }
    }
}