using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Repositories;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<StudentDto> GetStudentByIdAsync(string id)
        {
            var student = await _studentRepository.GetStudentByIdAsync(id);
            if (student == null)
            {
                throw new InvalidOperationException("Student not found.");
            }
            return new StudentDto
            {
                StudentId = student.StudentId,
                Name = student.Name,
                Email = student.Email
            };
        }

        public async Task UpdateStudentAsync(StudentDto studentDto)
        {
            var student = await _studentRepository.GetStudentByIdAsync(studentDto.StudentId);
            if (student == null)
            {
                throw new InvalidOperationException("Student not found.");
            }

            student.Name = studentDto.Name;
            student.Email = studentDto.Email;
            await _studentRepository.UpdateStudentAsync(student);
        }

        public async Task<string> UploadAvatarAsync(string studentId, IFormFile file)
        {
            var student = await _studentRepository.GetStudentByIdAsync(studentId);
            if (student == null)
            {
                throw new InvalidOperationException("Student not found.");
            }

            var fileName = $"{studentId}_{DateTime.Now.Ticks}{Path.GetExtension(file.FileName)}";
            var avatarUrl = $"/avatars/{fileName}"; 

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                // In a real app, save the stream to storage
            }

            student.AvatarUrl = avatarUrl;
            await _studentRepository.UpdateStudentAsync(student);
            return avatarUrl;
        }

        public async Task<List<StudentDto>> GetAllStudentsAsync(int page, int pageSize)
        {
            var students = await _studentRepository.GetAllStudentsAsync(page, pageSize);
            return students.Select(s => new StudentDto
            {
                StudentId = s.StudentId,
                Name = s.Name,
                Email = s.Email
            }).ToList();
        }

        public async Task<List<StudentDto>> GetAllStudentsAsync()
        {
            var students = await _studentRepository.GetAllStudentsAsync();
            return students.Select(s => new StudentDto
            {
                StudentId = s.StudentId,
                Name = s.Name,
                Email = s.Email
            }).ToList();
        }

        public async Task<int> GetTotalStudentsCountAsync()
        {
            return await _studentRepository.GetTotalStudentsCountAsync();
        }
    }
}