using System;
using DataAccessLayer;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLayer
{
    public static class DIResolver
    {
        public static IServiceCollection RegisterBusinessDependencies(this IServiceCollection services)
        {
            services.RegisterDatabaseDependencies();
            services.AddScoped<IBAO, BAO>();
            return services;
        }
    }
}
