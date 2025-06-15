using ENROLLMENTSYSTEMBACKEND.Data;
using ENROLLMENTSYSTEMBACKEND.Repositories;
using ENROLLMENTSYSTEMBACKEND.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace ENROLLMENTSYSTEMBACKEND.Microservices.FormsService
{
    public class FormsServiceStartup
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // Add services to the container
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Student Forms API", Version = "v1" });
            });

            // Configure database
            services.AddDbContext<EnrollmentInformationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Register repositories
            services.AddScoped<IFormRepository, FormRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IGradeRepository, GradeRepository>();

            // Register services
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IEmailService, EmailService>();

            // Add CORS
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            // Add health checks
            services.AddHealthChecks();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configure the HTTP request pipeline
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}