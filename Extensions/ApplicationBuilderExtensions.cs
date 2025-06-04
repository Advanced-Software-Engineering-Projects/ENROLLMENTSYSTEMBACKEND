using Microsoft.AspNetCore.Builder;
using ENROLLMENTSYSTEMBACKEND.Services;

namespace ENROLLMENTSYSTEMBACKEND.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseServiceAccessControl(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ServiceAccessMiddleware>();
        }
    }
}