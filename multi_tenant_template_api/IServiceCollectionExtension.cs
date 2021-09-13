using BusinessLayer;
using Common;
using Microsoft.Extensions.DependencyInjection;

namespace CMS.Api
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddDependencyServices(this IServiceCollection services)
        {
            services.AddBusinessLayer();
            services.AddCommonLibrary();
            return services;
        }
    }
}
