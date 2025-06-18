using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Models;
using ENROLLMENTSYSTEMBACKEND.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public class ProgramService : IProgramService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IProgramRepository _programRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;

        public ProgramService(
            IStudentRepository studentRepository,
            IProgramRepository programRepository,
            ICourseRepository courseRepository,
            IEnrollmentRepository enrollmentRepository)
        {
            _studentRepository = studentRepository;
            _programRepository = programRepository;
            _courseRepository = courseRepository;
            _enrollmentRepository = enrollmentRepository;
        }

        public async Task<ProgramAuditDto> GetProgramAuditAsync(string studentId)
        {
            var student = await _studentRepository.GetStudentByIdAsync(studentId);
            if (student == null)
            {
                throw new InvalidOperationException("Student not found.");
            }

            var program = await _programRepository.GetProgramByIdAsync(student.ProgramId);
            if (program == null)
            {
                throw new InvalidOperationException("Program not found.");
            }

            var enrollments = await _enrollmentRepository.GetEnrollmentsByStudentIdAsync(studentId);
            var courseStatuses = new List<CourseStatusDto>();

            int completedCourses = 0;
            foreach (var courseId in program.RequiredCourseIds)
            {
                var course = await _courseRepository.GetCourseByIdAsync(courseId);
                if (course == null) continue;

                var enrollment = enrollments.FirstOrDefault(e => e.CourseId == courseId);
                string status = enrollment != null ? enrollment.Status : "Not Started";
                string grade = enrollment?.Grade ?? "N/A";

                if (status == "Completed") completedCourses++;

                courseStatuses.Add(new CourseStatusDto
                {
                    CourseId = course.CourseId,
                    CourseName = course.CourseName,
                    Status = status,
                    Grade = grade
                });
            }

            double completionProgress = program.RequiredCourseIds.Count > 0
                ? (double)completedCourses / program.RequiredCourseIds.Count * 100
                : 0;

            return new ProgramAuditDto
            {
                StudentId = student.StudentId,
                StudentName = student.Name,
                ProgramName = program.Name,
                CourseStatuses = courseStatuses,
                CompletionProgress = completionProgress
            };
        }

        public async Task<StudentDto?> GetStudentByIdAsync(string studentId)
        {
            var student = await _studentRepository.GetStudentByIdAsync(studentId);
            if (student == null)
            {
                return null;
            }

            return new StudentDto
            {
                StudentId = student.StudentId,
                Name = student.Name,
                FirstName = student.FirstName,
                MiddleName = student.MiddleName,
                LastName = student.LastName,
                Dob = student.Dob,
                Email = student.Email,
                Phone = student.Phone,
                Avatar = student.Avatar,
                Gender = student.Gender,
                Citizenship = student.Citizenship,
                Program = student.Program,
                StudentLevel = student.StudentLevel,
                StudentCampus = student.StudentCampus,
                ExamSite = student.ExamSite,
                MajorType = student.MajorType,
                Major1 = student.Major1,
                Major2 = student.Major2
            };
        }
    }
}
