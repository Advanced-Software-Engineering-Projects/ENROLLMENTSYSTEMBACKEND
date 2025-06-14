using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Security.Claims;
using System;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public class ServiceAccessMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly HoldManagementServiceClient _holdClient;

        public ServiceAccessMiddleware(RequestDelegate next, HoldManagementServiceClient holdClient)
        {
            _next = next;
            _holdClient = holdClient;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Skip middleware for non-student users or for hold-related endpoints
            if (!context.User.IsInRole("Student") || 
                context.Request.Path.StartsWithSegments("/api/students/holds") ||
                context.Request.Path.StartsWithSegments("/api/auth"))
            {
                await _next(context);
                return;
            }

            // Get student ID from claims
            var studentId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(studentId))
            {
                await _next(context);
                return;
            }

            // Determine which service is being accessed based on the path
            string? serviceCode = GetServiceCodeFromPath(context.Request.Path);
            if (string.IsNullOrEmpty(serviceCode))
            {
                await _next(context);
                return;
            }

            // Check if student has access to the service
            var response = await _holdClient.CheckServiceAccessAsync(studentId, serviceCode);
            if (!response.IsSuccessStatusCode)
            {
                // If service check fails, allow access (fail open for availability)
                await _next(context);
                return;
            }

            var content = await response.Content.ReadAsStringAsync();
            var result = System.Text.Json.JsonSerializer.Deserialize<ServiceAccessResult>(content, 
                new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (result == null || result.HasAccess)
            {
                await _next(context);
                return;
            }

            // Deny access if student is on hold and service is restricted
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsJsonAsync(new 
            { 
                error = "Access Denied", 
                message = "You currently have a hold on your account. Please check your holds for more information." 
            });
        }

        private string? GetServiceCodeFromPath(PathString path)
        {
            if (path.StartsWithSegments("/api/course-registrations"))
                return "course_registration";
            else if (path.StartsWithSegments("/api/grades"))
                return "view_course_grades";
            else if (path.StartsWithSegments("/api/programs"))
                return "view_programme_structure";
            else if (path.StartsWithSegments("/api/forms/grade-recheck"))
                return "apply_grade_recheck";
            else if (path.StartsWithSegments("/api/forms/graduation"))
                return "apply_graduation";
            
            return null;
        }

        private class ServiceAccessResult
        {
            public bool HasAccess { get; set; }
        }
    }
}