using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Models;
using ENROLLMENTSYSTEMBACKEND.Repositories;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public class HoldService : IHoldService
    {
        private readonly IHoldRepository _holdRepository;
        private readonly IStudentRepository _studentRepository;

        public HoldService(IHoldRepository holdRepository, IStudentRepository studentRepository)
        {
            _holdRepository = holdRepository;
            _studentRepository = studentRepository;
        }

        public async Task<List<HoldResponseDto>> GetHoldsAsync(string? studentId)
        {
            var holds = await _holdRepository.GetHoldsAsync(studentId);
            return holds.Select(h => new HoldResponseDto
            {
                Id = h.Id,
                StudentId = h.StudentId,
                Service = h.Service,
                Reason = h.Reason,
                CreatedAt = h.CreatedAt
            }).ToList();
        }

        public async Task<HoldResponseDto> AddHoldAsync(HoldDto holdDto)
        {
            var student = await _studentRepository.GetStudentByIdAsync(holdDto.StudentId);
            if (student == null)
            {
                throw new InvalidOperationException("Student not found.");
            }

            var hold = new Hold
            {
                StudentId = holdDto.StudentId,
                Service = holdDto.Service,
                Reason = holdDto.Reason
            };

            await _holdRepository.AddHoldAsync(hold);

            return new HoldResponseDto
            {
                Id = hold.Id,
                StudentId = hold.StudentId,
                Service = hold.Service,
                Reason = hold.Reason,
                CreatedAt = hold.CreatedAt
            };
        }

        public async Task RemoveHoldAsync(string id)
        {
            var hold = await _holdRepository.GetHoldByIdAsync(id);
            if (hold == null)
            {
                throw new InvalidOperationException("Hold not found.");
            }

            await _holdRepository.RemoveHoldAsync(id);
        }

        public async Task<bool> HasHoldsAsync(string studentId)
        {
            var holds = await _holdRepository.GetHoldsAsync(studentId);
            return holds.Any();
        }
    }
}