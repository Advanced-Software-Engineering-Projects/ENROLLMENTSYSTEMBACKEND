using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using ExternalFormIntegrationService.Services;

namespace ExternalFormIntegrationService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ExternalFormsController : ControllerBase
    {
        private const string FormsDataFile = "formsData.json";
        private readonly NotificationService _notificationService;

        public ExternalFormsController()
        {
            _notificationService = new NotificationService();
        }

        [HttpGet("forms")]
        public IActionResult GetForms()
        {
            if (!System.IO.File.Exists(FormsDataFile))
            {
                return Ok(new List<Form>());
            }

            var json = System.IO.File.ReadAllText(FormsDataFile);
            var forms = JsonSerializer.Deserialize<List<Form>>(json);
            return Ok(forms);
        }

        [HttpPost("apply")]
        public IActionResult ApplyForForm([FromBody] FormApplication application)
        {
            if (application == null || string.IsNullOrEmpty(application.StudentId) || string.IsNullOrEmpty(application.FormType))
            {
                return BadRequest("Invalid application data.");
            }

            var forms = new List<Form>();
            if (System.IO.File.Exists(FormsDataFile))
            {
                var json = System.IO.File.ReadAllText(FormsDataFile);
                forms = JsonSerializer.Deserialize<List<Form>>(json);
            }

            var newForm = new Form
            {
                Id = System.Guid.NewGuid().ToString(),
                StudentId = application.StudentId,
                FormType = application.FormType,
                Status = "Pending"
            };

            forms.Add(newForm);

            var updatedJson = JsonSerializer.Serialize(forms);
            System.IO.File.WriteAllText(FormsDataFile, updatedJson);

            // Send notification and email if qualified (simulate)
            _notificationService.SendNotification(application.StudentId, $"Your application for {application.FormType} has been received.");
            _notificationService.SendEmail(application.StudentId, $"Application for {application.FormType}", "Your application is under review.");

            return Ok(new { Message = "Application submitted.", Status = "Pending" });
        }
    }

    public class Form
    {
        public string Id { get; set; }
        public string StudentId { get; set; }
        public string FormType { get; set; }
        public string Status { get; set; }
    }

    public class FormApplication
    {
        public string StudentId { get; set; }
        public string FormType { get; set; }
    }
}
