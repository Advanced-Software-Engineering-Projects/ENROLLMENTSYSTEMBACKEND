using ENROLLMENTSYSTEMBACKEND.DTOs;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public class TranscriptPdfService
    {
        public byte[] GenerateTranscriptPdf(TranscriptDto transcript)
        {
            using (var document = new PdfDocument())
            {
                var page = document.AddPage();
                var gfx = XGraphics.FromPdfPage(page);
                var fontTitle = new XFont("Verdana", 20, XFontStyle.Bold);
                var fontHeader = new XFont("Verdana", 12, XFontStyle.Bold);
                var fontRegular = new XFont("Verdana", 10, XFontStyle.Regular);

                double yPoint = 40;

                // Title
                gfx.DrawString("Academic Transcript", fontTitle, XBrushes.Black,
                    new XRect(0, yPoint, page.Width, 40), XStringFormats.TopCenter);
                yPoint += 50;

                // Student ID and GPA
                gfx.DrawString($"Student ID: {transcript.StudentId}", fontHeader, XBrushes.Black,
                    new XRect(40, yPoint, page.Width, 20), XStringFormats.TopLeft);
                yPoint += 25;

                gfx.DrawString($"GPA: {transcript.GPA:F2}", fontHeader, XBrushes.Black,
                    new XRect(40, yPoint, page.Width, 20), XStringFormats.TopLeft);
                yPoint += 30;

                // Table headers
                gfx.DrawString("Course Code", fontHeader, XBrushes.Black, 40, yPoint);
                gfx.DrawString("Course Name", fontHeader, XBrushes.Black, 140, yPoint);
                gfx.DrawString("Semester", fontHeader, XBrushes.Black, 400, yPoint);
                gfx.DrawString("Grade", fontHeader, XBrushes.Black, 500, yPoint);
                yPoint += 20;

                // Draw a line under headers
                gfx.DrawLine(XPens.Black, 40, yPoint, page.Width - 40, yPoint);
                yPoint += 10;

                // List enrollments
                foreach (var enrollment in transcript.Enrollments)
                {
                    if (yPoint > page.Height - 50)
                    {
                        // Add new page if space is insufficient
                        page = document.AddPage();
                        gfx = XGraphics.FromPdfPage(page);
                        yPoint = 40;
                    }

                    gfx.DrawString(enrollment.CourseCode ?? "", fontRegular, XBrushes.Black, 40, yPoint);
                    gfx.DrawString(enrollment.CourseName ?? "", fontRegular, XBrushes.Black, 140, yPoint);
                    gfx.DrawString(enrollment.Semester ?? "", fontRegular, XBrushes.Black, 400, yPoint);
                    gfx.DrawString(enrollment.Grade ?? "", fontRegular, XBrushes.Black, 500, yPoint);
                    yPoint += 20;
                }

                using (var stream = new MemoryStream())
                {
                    document.Save(stream, false);
                    return stream.ToArray();
                }
            }
        }
    }
}
