using ENROLLMENTSYSTEMBACKEND.DTOs;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.IO;
using System.Linq;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public class TranscriptPdfService
    {
        public byte[] GenerateTranscriptPdf(TranscriptDto transcript)
        {
            // Configure QuestPDF
            QuestPDF.Settings.License = LicenseType.Community;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(50);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header().Element(header =>
                    {
                        header.AlignCenter().Text("Academic Transcript")
                            .FontSize(20).Bold();
                    });

                    page.Content().Element(content =>
                    {
                        content.Column(column =>
                        {
                            // Student information
                            column.Item().PaddingVertical(10).Row(row =>
                            {
                                row.RelativeItem().Column(c =>
                                {
                                    c.Item().Text($"Student ID: {transcript.StudentId}").Bold();
                                    c.Item().Text($"GPA: {transcript.GPA:F2}").Bold();
                                });
                            });

                            // Enrollments table
                            column.Item().Table(table =>
                            {
                                // Define columns
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(4);
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(1);
                                });

                                // Add header row
                                table.Header(header =>
                                {
                                    header.Cell().Text("Course Code").Bold();
                                    header.Cell().Text("Course Name").Bold();
                                    header.Cell().Text("Semester").Bold();
                                    header.Cell().Text("Grade").Bold();
                                });

                                // Add data rows
                                foreach (var enrollment in transcript.Enrollments)
                                {
                                    table.Cell().Text(enrollment.CourseCode ?? "");
                                    table.Cell().Text(enrollment.CourseName ?? "");
                                    table.Cell().Text(enrollment.Semester ?? "");
                                    table.Cell().Text(enrollment.Grade ?? "");
                                }
                            });
                        });
                    });

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Page ");
                        x.CurrentPageNumber();
                        x.Span(" of ");
                        x.TotalPages();
                    });
                });
            });

            using (var stream = new MemoryStream())
            {
                document.GeneratePdf(stream);
                return stream.ToArray();
            }
        }
    }
}
