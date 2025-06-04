using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ENROLLMENTSYSTEMBACKEND.HoldManagementService
{
    // Models
    public class StudentHold
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string StudentId { get; set; }
        public string HoldType { get; set; } = "FeeNonPayment"; // Default to fee non-payment
        public string Reason { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
    }

    public class HoldServiceAccess
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string HoldType { get; set; } = "FeeNonPayment"; // Default to fee non-payment
        public Dictionary<string, bool> AllowedServices { get; set; } = new Dictionary<string, bool>();
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        public string UpdatedBy { get; set; }
    }

    public class ServiceDefinition
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string ServiceName { get; set; }
        public string ServiceDescription { get; set; }
        public string ServiceCode { get; set; }
    }

    public static class StudentServices
    {
        public const string CourseRegistration = "course_registration";
        public const string ViewCourseGrades = "view_course_grades";
        public const string ViewProgrammeStructure = "view_programme_structure";
        public const string ApplyForGradeRecheck = "apply_grade_recheck";
        public const string ApplyForGraduation = "apply_graduation";
        
        public static List<ServiceDefinition> GetAllServices()
        {
            return new List<ServiceDefinition>
            {
                new ServiceDefinition { 
                    ServiceName = "Course Registration", 
                    ServiceDescription = "Register for courses during registration periods",
                    ServiceCode = CourseRegistration
                },
                new ServiceDefinition { 
                    ServiceName = "View Course Grades", 
                    ServiceDescription = "Access and view course grades",
                    ServiceCode = ViewCourseGrades
                },
                new ServiceDefinition { 
                    ServiceName = "View Programme Structure", 
                    ServiceDescription = "View program curriculum and requirements",
                    ServiceCode = ViewProgrammeStructure
                },
                new ServiceDefinition { 
                    ServiceName = "Apply for Grade Recheck", 
                    ServiceDescription = "Submit applications for grade rechecks",
                    ServiceCode = ApplyForGradeRecheck
                },
                new ServiceDefinition { 
                    ServiceName = "Apply for Graduation", 
                    ServiceDescription = "Submit graduation applications",
                    ServiceCode = ApplyForGraduation
                }
            };
        }
    }

    // Data Access
    public class HoldDbContext
    {
        private readonly string _holdServiceAccessPath;
        private readonly string _studentHoldsPath;
        private readonly JsonSerializerOptions _jsonOptions;

        public HoldDbContext(string dataDirectory)
        {
            // Ensure directory exists
            if (!Directory.Exists(dataDirectory))
            {
                Directory.CreateDirectory(dataDirectory);
            }

            _holdServiceAccessPath = Path.Combine(dataDirectory, "hold_service_access.json");
            _studentHoldsPath = Path.Combine(dataDirectory, "student_holds.json");
            
            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            // Initialize files if they don't exist
            InitializeFiles();
        }

        private void InitializeFiles()
        {
            // Initialize hold service access file with default settings
            if (!File.Exists(_holdServiceAccessPath))
            {
                var defaultHoldServiceAccess = new HoldServiceAccess
                {
                    HoldType = "FeeNonPayment",
                    AllowedServices = new Dictionary<string, bool>
                    {
                        { StudentServices.CourseRegistration, false },
                        { StudentServices.ViewCourseGrades, true },
                        { StudentServices.ViewProgrammeStructure, true },
                        { StudentServices.ApplyForGradeRecheck, false },
                        { StudentServices.ApplyForGraduation, false }
                    },
                    UpdatedBy = "System"
                };

                string json = JsonSerializer.Serialize(defaultHoldServiceAccess, _jsonOptions);
                File.WriteAllText(_holdServiceAccessPath, json);
            }

            // Initialize student holds file with empty array
            if (!File.Exists(_studentHoldsPath))
            {
                File.WriteAllText(_studentHoldsPath, "[]");
            }
        }

        public HoldServiceAccess GetHoldServiceAccess()
        {
            string json = File.ReadAllText(_holdServiceAccessPath);
            return JsonSerializer.Deserialize<HoldServiceAccess>(json);
        }

        public void UpdateHoldServiceAccess(HoldServiceAccess holdServiceAccess)
        {
            holdServiceAccess.LastUpdated = DateTime.UtcNow;
            string json = JsonSerializer.Serialize(holdServiceAccess, _jsonOptions);
            File.WriteAllText(_holdServiceAccessPath, json);
        }

        public List<StudentHold> GetStudentHolds()
        {
            string json = File.ReadAllText(_studentHoldsPath);
            return JsonSerializer.Deserialize<List<StudentHold>>(json) ?? new List<StudentHold>();
        }

        public List<StudentHold> GetStudentHoldsByStudentId(string studentId)
        {
            var holds = GetStudentHolds();
            return holds.Where(h => h.StudentId == studentId && h.IsActive).ToList();
        }

        public StudentHold GetStudentHoldById(string holdId)
        {
            var holds = GetStudentHolds();
            return holds.FirstOrDefault(h => h.Id == holdId);
        }

        public void AddStudentHold(StudentHold studentHold)
        {
            var holds = GetStudentHolds();
            holds.Add(studentHold);
            SaveStudentHolds(holds);
        }

        public void UpdateStudentHold(StudentHold updatedHold)
        {
            var holds = GetStudentHolds();
            var index = holds.FindIndex(h => h.Id == updatedHold.Id);
            
            if (index != -1)
            {
                holds[index] = updatedHold;
                SaveStudentHolds(holds);
            }
        }

        public void RemoveStudentHold(string holdId)
        {
            var holds = GetStudentHolds();
            var hold = holds.FirstOrDefault(h => h.Id == holdId);
            
            if (hold != null)
            {
                hold.IsActive = false;
                SaveStudentHolds(holds);
            }
        }

        private void SaveStudentHolds(List<StudentHold> holds)
        {
            string json = JsonSerializer.Serialize(holds, _jsonOptions);
            File.WriteAllText(_studentHoldsPath, json);
        }
    }

    // Service Layer
    public interface IHoldService
    {
        HoldServiceAccess GetHoldServiceAccessRules();
        void UpdateHoldServiceAccessRules(HoldServiceAccess holdServiceAccess);
        List<StudentHold> GetAllStudentHolds();
        List<StudentHold> GetStudentHoldsByStudentId(string studentId);
        StudentHold GetStudentHoldById(string holdId);
        void AddStudentHold(StudentHold studentHold);
        void UpdateStudentHold(StudentHold studentHold);
        void RemoveStudentHold(string holdId);
        bool CheckServiceAccess(string studentId, string serviceCode);
    }

    public class HoldService : IHoldService
    {
        private readonly HoldDbContext _dbContext;

        public HoldService(HoldDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public HoldServiceAccess GetHoldServiceAccessRules()
        {
            return _dbContext.GetHoldServiceAccess();
        }

        public void UpdateHoldServiceAccessRules(HoldServiceAccess holdServiceAccess)
        {
            _dbContext.UpdateHoldServiceAccess(holdServiceAccess);
        }

        public List<StudentHold> GetAllStudentHolds()
        {
            return _dbContext.GetStudentHolds().Where(h => h.IsActive).ToList();
        }

        public List<StudentHold> GetStudentHoldsByStudentId(string studentId)
        {
            return _dbContext.GetStudentHoldsByStudentId(studentId);
        }

        public StudentHold GetStudentHoldById(string holdId)
        {
            return _dbContext.GetStudentHoldById(holdId);
        }

        public void AddStudentHold(StudentHold studentHold)
        {
            _dbContext.AddStudentHold(studentHold);
        }

        public void UpdateStudentHold(StudentHold studentHold)
        {
            _dbContext.UpdateStudentHold(studentHold);
        }

        public void RemoveStudentHold(string holdId)
        {
            _dbContext.RemoveStudentHold(holdId);
        }

        public bool CheckServiceAccess(string studentId, string serviceCode)
        {
            // Get student holds
            var studentHolds = GetStudentHoldsByStudentId(studentId);
            
            // If no holds, student has access to all services
            if (studentHolds == null || !studentHolds.Any())
            {
                return true;
            }

            // Get hold service access rules
            var accessRules = GetHoldServiceAccessRules();
            
            // Check if the service is allowed during hold
            if (accessRules.AllowedServices.TryGetValue(serviceCode, out bool isAllowed))
            {
                return isAllowed;
            }
            
            // Default to false if service is not defined in rules
            return false;
        }
    }

    // API Controller
    [ApiController]
    [Route("api/[controller]")]
    public class HoldManagementController : ControllerBase
    {
        private readonly IHoldService _holdService;

        public HoldManagementController(IHoldService holdService)
        {
            _holdService = holdService;
        }

        [HttpGet("service-access-rules")]
        public IActionResult GetServiceAccessRules()
        {
            var rules = _holdService.GetHoldServiceAccessRules();
            return Ok(rules);
        }

        [HttpPut("service-access-rules")]
        public IActionResult UpdateServiceAccessRules([FromBody] HoldServiceAccess accessRules)
        {
            if (accessRules == null)
            {
                return BadRequest("Access rules cannot be null");
            }

            _holdService.UpdateHoldServiceAccessRules(accessRules);
            return Ok();
        }

        [HttpGet("services")]
        public IActionResult GetAvailableServices()
        {
            var services = StudentServices.GetAllServices();
            return Ok(services);
        }

        [HttpGet("student-holds")]
        public IActionResult GetAllHolds()
        {
            var holds = _holdService.GetAllStudentHolds();
            return Ok(holds);
        }

        [HttpGet("student-holds/student/{studentId}")]
        public IActionResult GetStudentHolds(string studentId)
        {
            if (string.IsNullOrEmpty(studentId))
            {
                return BadRequest("Student ID is required");
            }

            var holds = _holdService.GetStudentHoldsByStudentId(studentId);
            return Ok(holds);
        }

        [HttpGet("student-holds/{holdId}")]
        public IActionResult GetHoldById(string holdId)
        {
            if (string.IsNullOrEmpty(holdId))
            {
                return BadRequest("Hold ID is required");
            }

            var hold = _holdService.GetStudentHoldById(holdId);
            if (hold == null)
            {
                return NotFound();
            }

            return Ok(hold);
        }

        [HttpPost("student-holds")]
        public IActionResult AddHold([FromBody] StudentHold hold)
        {
            if (hold == null)
            {
                return BadRequest("Hold data is required");
            }

            if (string.IsNullOrEmpty(hold.StudentId))
            {
                return BadRequest("Student ID is required");
            }

            _holdService.AddStudentHold(hold);
            return CreatedAtAction(nameof(GetHoldById), new { holdId = hold.Id }, hold);
        }

        [HttpPut("student-holds/{holdId}")]
        public IActionResult UpdateHold(string holdId, [FromBody] StudentHold hold)
        {
            if (hold == null)
            {
                return BadRequest("Hold data is required");
            }

            if (string.IsNullOrEmpty(holdId))
            {
                return BadRequest("Hold ID is required");
            }

            var existingHold = _holdService.GetStudentHoldById(holdId);
            if (existingHold == null)
            {
                return NotFound();
            }

            hold.Id = holdId;
            _holdService.UpdateStudentHold(hold);
            return NoContent();
        }

        [HttpDelete("student-holds/{holdId}")]
        public IActionResult RemoveHold(string holdId)
        {
            if (string.IsNullOrEmpty(holdId))
            {
                return BadRequest("Hold ID is required");
            }

            var existingHold = _holdService.GetStudentHoldById(holdId);
            if (existingHold == null)
            {
                return NotFound();
            }

            _holdService.RemoveStudentHold(holdId);
            return NoContent();
        }

        [HttpGet("integration/check-access")]
        public IActionResult CheckServiceAccess([FromQuery] string studentId, [FromQuery] string service, [FromQuery] string apiKey)
        {
            // In a production environment, validate the API key
            if (string.IsNullOrEmpty(apiKey) || apiKey != "your-secure-api-key")
            {
                return Unauthorized();
            }

            if (string.IsNullOrEmpty(studentId) || string.IsNullOrEmpty(service))
            {
                return BadRequest("Student ID and service are required");
            }

            bool hasAccess = _holdService.CheckServiceAccess(studentId, service);
            return Ok(new { HasAccess = hasAccess });
        }
    }

    // Program
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Configure CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            // Configure data directory
            string dataDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Data", "JsonDb");
            builder.Services.AddSingleton(new HoldDbContext(dataDirectory));

            // Register services
            builder.Services.AddScoped<IHoldService, HoldService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.UseAuthorization();
            app.MapControllers();

            app.Run("http://localhost:5003");
        }
    }
}