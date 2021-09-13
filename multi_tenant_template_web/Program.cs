using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using CMS.Web.Services;
using CMS.Web.Helpers;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;

namespace Multi_Tenant_Template.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.Services.AddMudServices();

            builder.Services.AddScoped(x => {
                var apiUrl = new Uri(builder.Configuration["apiUrl"]);
                return new HttpClient() { BaseAddress = apiUrl };
            });

            builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvidor>();
            builder.Services.AddApiAuthorization();

            builder.Services
                .AddScoped<ISecurityService, SecurityService>()
                .AddScoped<IAuthenticationService, AuthenticationService>()
                .AddScoped<IHttpService, HttpService>();


						await builder.Build().RunAsync();
        }
    }
}
