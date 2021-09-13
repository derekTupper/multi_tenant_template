using Microsoft.Extensions.DependencyInjection;
using Common.Helpers;


namespace Common
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection  AddCommonLibrary(this IServiceCollection services)
        {
            services.AddScoped<Logging>();
            services.AddScoped<ITokenManager, TokenManager>();
            return services;
        }
    }
}
