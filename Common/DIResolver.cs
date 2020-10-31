using System;
using Microsoft.Extensions.DependencyInjection;

namespace Common
{
    public static class DIResolver
    {
        public static IServiceCollection RegisterCommonUtilsDependencies(this IServiceCollection services)
        {
            //services.AddScoped<IBAO, BAO>();
            return services;
        }
    }
}
