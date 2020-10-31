using System;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccessLayer
{
    public static class DIResolver
    {
        public static IServiceCollection RegisterDatabaseDependencies(this IServiceCollection services)
        {
            services.AddScoped<IDAO, DAO>();
            return services;
        }
    }
}
