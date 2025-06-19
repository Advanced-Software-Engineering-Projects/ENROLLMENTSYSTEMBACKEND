using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Models;
using ENROLLMENTSYSTEMBACKEND.Repositories;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public class RegistrationPeriodService : IRegistrationPeriodService
    {
        private readonly IRegistrationPeriodRepository _registrationPeriodRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;

        public RegistrationPeriodService(
            IRegistrationPeriodRepository registrationPeriodRepository,
            IEnrollmentRepository enrollmentRepository)
        {
            _registrationPeriodRepository = registrationPeriodRepository;
            _enrollmentRepository = enrollmentRepository;
        }

        public async Task OpenRegistrationAsync(DateTime startDate, DateTime endDate)
        {
            var currentPeriod = await _registrationPeriodRepository.GetCurrentRegistrationPeriodAsync();
            if (currentPeriod != null && currentPeriod.IsActive)
            {
                throw new InvalidOperationException("Registration period already open.");
            }

            var newPeriod = new RegistrationPeriod
            {
                RegistrationPeriodId = Guid.NewGuid().ToString(),
                StartDate = startDate,
                EndDate = endDate,
                IsActive = true
            };

            await _registrationPeriodRepository.AddRegistrationPeriodAsync(newPeriod);
        }

        public async Task CloseRegistrationAsync()
        {
            var currentPeriod = await _registrationPeriodRepository.GetCurrentRegistrationPeriodAsync();
            if (currentPeriod == null || !currentPeriod.IsActive)
            {
                throw new InvalidOperationException("No active registration period.");
            }

            currentPeriod.IsActive = false;
            await _registrationPeriodRepository.UpdateRegistrationPeriodAsync(currentPeriod);
        }

        public async Task<RegistrationStatusDto> GetRegistrationStatusAsync()
        {
            var currentPeriod = await _registrationPeriodRepository.GetCurrentRegistrationPeriodAsync();
            if (currentPeriod == null)
            {
                return new RegistrationStatusDto { IsOpen = false };
            }

            return new RegistrationStatusDto
            {
                IsOpen = currentPeriod.IsActive,
                StartDate = currentPeriod.StartDate,
                EndDate = currentPeriod.EndDate
            };
        }

        public async Task<List<RegistrationPeriodDto>> GetAllRegistrationPeriodsAsync()
        {
            var periods = await _registrationPeriodRepository.GetAllRegistrationPeriodsAsync();
            return periods.Select(p => new RegistrationPeriodDto
            {
                StartDate = p.StartDate,
                EndDate = p.EndDate
            }).ToList();
        }

        public async Task<RegistrationPeriodDto?> GetCurrentRegistrationPeriodAsync()
        {
            var currentPeriod = await _registrationPeriodRepository.GetCurrentRegistrationPeriodAsync();
            if (currentPeriod == null)
            {
                return null;
            }

            return new RegistrationPeriodDto
            {
                StartDate = currentPeriod.StartDate,
                EndDate = currentPeriod.EndDate
            };
        }

        public async Task OpenCourseRegistrationAsync(CourseRegistrationDto courseRegistrationDto)
        {
            var currentPeriod = await _registrationPeriodRepository.GetCurrentRegistrationPeriodAsync();
            if (currentPeriod != null && currentPeriod.IsActive)
            {
                throw new InvalidOperationException("Registration period already open.");
            }

            var newPeriod = new RegistrationPeriod
            {
                RegistrationPeriodId = Guid.NewGuid().ToString(),
                StartDate = courseRegistrationDto.StartDate,
                EndDate = courseRegistrationDto.EndDate,
                StartTime = courseRegistrationDto.StartTime,
                EndTime = courseRegistrationDto.EndTime,
                CourseCodes = courseRegistrationDto.CourseCodes,
                IsActive = true
            };

            await _registrationPeriodRepository.AddRegistrationPeriodAsync(newPeriod);
        }

        public async Task CloseCourseRegistrationAsync(List<string> courseCodes)
        {
            var currentPeriod = await _registrationPeriodRepository.GetCurrentRegistrationPeriodAsync();
            if (currentPeriod == null || !currentPeriod.IsActive)
            {
                throw new InvalidOperationException("No active registration period.");
            }

            currentPeriod.IsActive = false;
            await _registrationPeriodRepository.UpdateRegistrationPeriodAsync(currentPeriod);
        }

        public async Task<RegistrationMetricsDto> GetRegistrationMetricsAsync()
        {
            var enrollments = await _enrollmentRepository.GetEnrollmentsAsync();
            var totalRegistrations = enrollments.Count(e => e.Status == "Registered");
            var droppedCourses = enrollments.Count(e => e.Status == "Dropped");

            return new RegistrationMetricsDto
            {
                TotalRegistrations = totalRegistrations,
                DroppedCourses = droppedCourses
            };
        }
    }
}