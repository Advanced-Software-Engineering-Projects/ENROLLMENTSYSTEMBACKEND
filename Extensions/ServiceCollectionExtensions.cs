using Microsoft.Extensions.DependencyInjection;
using ENROLLMENTSYSTEMBACKEND.Services;

namespace ENROLLMENTSYSTEMBACKEND.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServiceAccessMiddleware(this IServiceCollection services)
        {
            return services;
        }
    }
}