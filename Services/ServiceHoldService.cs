using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Models;
using ENROLLMENTSYSTEMBACKEND.Repositories;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public class ServiceHoldService : IServiceHoldService
    {
        private readonly IServiceHoldRepository _serviceHoldRepository;
        private readonly ILogger<ServiceHoldService> _logger;

        public ServiceHoldService(
            IServiceHoldRepository serviceHoldRepository,
            ILogger<ServiceHoldService> logger)
        {
            _serviceHoldRepository = serviceHoldRepository;
            _logger = logger;
        }

        public async Task<List<StudentWithHoldsDto>> GetAllStudentsAsync()
        {
            var students = await _serviceHoldRepository.GetAllStudentsAsync();
            
            var result = new List<StudentWithHoldsDto>();
            foreach (var student in students)
            {
                var holds = await _serviceHoldRepository.GetHoldsByStudentIdAsync(student.StudentId);
                result.Add(new StudentWithHoldsDto
                {
                    StudentId = student.StudentId,
                    Name = $"{student.FirstName} {student.LastName}",
                    Email = student.Email,
                    Holds = holds.Select(MapToHoldDto).ToList()
                });
            }
            
            return result;
        }

        public async Task<StudentWithHoldsDto> GetStudentWithHoldsAsync(string studentId)
        {
            var student = await _serviceHoldRepository.GetStudentByIdAsync(studentId);
            if (student == null)
            {
                return null;
            }
            
            var holds = await _serviceHoldRepository.GetHoldsByStudentIdAsync(studentId);
            
            return new StudentWithHoldsDto
            {
                StudentId = student.StudentId,
                Name = $"{student.FirstName} {student.LastName}",
                Email = student.Email,
                Holds = holds.Select(MapToHoldDto).ToList()
            };
        }

        public async Task<ServiceHoldDto> AddHoldAsync(CreateServiceHoldDto holdDto)
        {
            var student = await _serviceHoldRepository.GetStudentByIdAsync(holdDto.StudentId);
            if (student == null)
            {
                throw new KeyNotFoundException($"Student with ID {holdDto.StudentId} not found");
            }
            
            var hold = new ServiceHold
            {
                StudentId = holdDto.StudentId,
                Service = holdDto.Service,
                Reason = holdDto.Reason
            };
            
            var addedHold = await _serviceHoldRepository.AddHoldAsync(hold);
            return MapToHoldDto(addedHold);
        }

        public async Task<bool> RemoveHoldAsync(string holdId)
        {
            return await _serviceHoldRepository.RemoveHoldAsync(holdId);
        }

        public async Task<byte[]> GenerateHoldsPdfAsync(string studentId)
        {
            var studentWithHolds = await GetStudentWithHoldsAsync(studentId);
            if (studentWithHolds == null)
            {
                throw new KeyNotFoundException($"Student with ID {studentId} not found");
            }

            // Using QuestPDF for PDF generation
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header().Element(ComposeHeader);
                    
                    page.Content().Element(container =>
                    {
                        container.Column(column =>
                        {
                            // Student information
                            column.Item().PaddingBottom(10).Text(text =>
                            {
                                text.Line($"Date: {DateTime.Now.ToShortDateString()}");
                                text.Line($"Student Name: {studentWithHolds.Name}");
                                text.Line($"Student ID: {studentWithHolds.StudentId}");
                                text.Line($"Email: {studentWithHolds.Email}");
                            });

                            // Holds table
                            column.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(4);
                                    columns.RelativeColumn(2);
                                });

                                // Table header
                                table.Header(header =>
                                {
                                    header.Cell().Background("#094c50").Padding(5).Text("Hold ID").FontColor(Colors.White);
                                    header.Cell().Background("#094c50").Padding(5).Text("Service").FontColor(Colors.White);
                                    header.Cell().Background("#094c50").Padding(5).Text("Reason").FontColor(Colors.White);
                                    header.Cell().Background("#094c50").Padding(5).Text("Created At").FontColor(Colors.White);
                                });

                                // Table content
                                foreach (var hold in studentWithHolds.Holds)
                                {
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(hold.HoldId);
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(hold.Service);
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(hold.Reason);
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(hold.CreatedAt);
                                }
                            });
                        });
                    });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Generated by Admin Portal");
                            x.CurrentPageNumber();
                        });
                });
            });

            return document.GeneratePdf();
        }

        private void ComposeHeader(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text("Student Holds Report")
                        .FontSize(20)
                        .SemiBold();
                });
            });
        }

        private ServiceHoldDto MapToHoldDto(ServiceHold hold)
        {
            return new ServiceHoldDto
            {
                HoldId = hold.HoldId,
                StudentId = hold.StudentId,
                Service = hold.Service,
                Reason = hold.Reason,
                CreatedAt = hold.CreatedAt.ToString("yyyy-MM-dd")
            };
        }
    }
}