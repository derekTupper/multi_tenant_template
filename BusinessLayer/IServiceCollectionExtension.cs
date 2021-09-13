using Microsoft.Extensions.DependencyInjection;
using BusinessLayer.Interfaces;
using BusinessLayer.Classes;

namespace BusinessLayer
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection  AddBusinessLayer(this IServiceCollection services)
        {
            services.AddScoped<ITenantService, TenantService>();
            services.AddScoped<ISecurityService, SecurityService>();
            return services;
        }
    }
}
