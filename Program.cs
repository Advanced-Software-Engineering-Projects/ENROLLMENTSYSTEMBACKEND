using ENROLLMENTSYSTEMBACKEND.Data;
using ENROLLMENTSYSTEMBACKEND.Repositories;
using ENROLLMENTSYSTEMBACKEND.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Services;
using System.Text;

namespace ENROLLMENTSYSTEMBACKEND
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Enrollment System API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            // Configure database for EnrollmentInformationDbContext (consolidated)
            builder.Services.AddDbContext<EnrollmentInformationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
                
            // Configure JWT authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });

            // Register repositories
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IStudentRepository, StudentRepository>();
            builder.Services.AddScoped<ICourseRepository, CourseRepository>();
            builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
            builder.Services.AddScoped<IGradeRepository, GradeRepository>();
            builder.Services.AddScoped<IFeeRepository, FeeRepository>();
            builder.Services.AddScoped<IFeeHoldRepository, FeeHoldRepository>();
            builder.Services.AddScoped<IFormRepository, FormRepository>();
            builder.Services.AddScoped<IFormSubmissionRepository, FormSubmissionRepository>();
            builder.Services.AddScoped<IPaymentRecordRepository, PaymentRecordRepository>();
            builder.Services.AddScoped<IPendingRequestRepository, PendingRequestRepository>();
            builder.Services.AddScoped<IPrerequisiteRepository, PrerequisiteRepository>();
            builder.Services.AddScoped<IProgramRepository, ProgramRepository>();
            builder.Services.AddScoped<IRegistrationPeriodRepository, RegistrationPeriodRepository>();
            builder.Services.AddScoped<IServiceHoldRepository, ServiceHoldRepository>();
            builder.Services.AddScoped<IStudentRecordRepository, StudentRecordRepository>();
            builder.Services.AddScoped<ITimetableRepository, TimetableRepository>();
            builder.Services.AddScoped<IUserLogRepository, UserLogRepository>();
            builder.Services.AddScoped<ICourseManagementRepository, CourseManagementRepository>();
            builder.Services.AddScoped<IHoldRepository, HoldRepository>();

            // Register services
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IStudentService, StudentService>();
            builder.Services.AddScoped<ICourseService, CourseService>();
            builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
            builder.Services.AddScoped<IGradeService, GradeService>();
            builder.Services.AddScoped<IFeeService, FeeService>();
            builder.Services.AddScoped<IFormService, FormService>();
            builder.Services.AddScoped<IFormConfigurationService, FormConfigurationService>();
            builder.Services.AddScoped<IProgramService, ProgramService>();
            builder.Services.AddScoped<IRegistrationPeriodService, RegistrationPeriodService>();
            builder.Services.AddScoped<IServiceHoldService, ServiceHoldService>();
            builder.Services.AddScoped<IStudentRecordService, StudentRecordService>();
            builder.Services.AddScoped<ITimetableService, TimetableService>();
            builder.Services.AddScoped<ITranscriptService, TranscriptService>();
            builder.Services.AddScoped<IUserLogService, UserLogService>();
            builder.Services.AddScoped<ICourseManagementService, CourseManagementService>();
            builder.Services.AddScoped<IDashboardService, DashboardService>();
            builder.Services.AddScoped<IAdminDashboardService, AdminDashboardService>();
            builder.Services.AddScoped<IHoldService, HoldService>();
            builder.Services.AddScoped<IGradeRecheckService, GradeRecheckService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<IAutoFillService, AutoFillService>();

            // Register HTTP clients
            builder.Services.AddHttpClient<ExternalFormIntegrationServiceClient>();
            builder.Services.AddScoped<ExternalFormIntegrationServiceClient>();

            builder.Services.AddHttpClient<GradeRecheckServiceClient>(client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["ServiceEndpoints:GradeRecheckService"] ?? "http://grade-recheck-service/api/");
            });

            builder.Services.AddHttpClient<StudentFormsServiceClient>(client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["ServiceEndpoints:FormsService"] ?? "http://forms-service/api/");
            });

            builder.Services.AddHttpClient("FormsService", client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["Microservices:FormsServiceUrl"]);
            });

            // Add CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            // Add session support
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            app.MapControllers();
            
            // Redirect root to Swagger UI
            app.MapGet("/", () => Results.Redirect("/swagger"));

            app.Run();
        }
    }
}