using BusinessLayer;
using BusinessLayer.Interfaces;
using Common.Models;
using DataLayer;
using DataLayer.Entities.Tenant;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace CMS.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
         


            var jwtTokenConfig = Configuration.GetSection("jwtTokenConfig").Get<JwtTokenConfig>();
            services.AddSingleton(jwtTokenConfig);
            services.AddControllers();
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            
            }).AddJwtBearer(x => 
            {
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidIssuer = jwtTokenConfig.Issuer,
                    ValidateIssuerSigningKey = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtTokenConfig.Secret)),
                    ValidAudience = jwtTokenConfig.Audience,
                    ValidateAudience = false, 
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMilliseconds(1)
                };          
            });

            services.AddCors(policy =>
            {
            policy.AddPolicy("CorsPolicy", opt => opt
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());
            });


            services.AddSwaggerGen();
         
            services.AddDependencyServices();

            services.AddDbContext<PlatformContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("PlatformContext")));

            services.AddDbContext<TenantContext>();

            services.AddMultiTenant<TenantModel>()
                    .WithInMemoryStore()
                    .WithClaimStrategy("TenantID")
                    .WithHeaderStrategy();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ITenantService tenant)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CMS.V1");
                });
            }
            app.UseSwagger();
            app.UseMultiTenant();
            app.UseHttpsRedirection();
            app.UseCors("CorsPolicy");
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(x => x.MapControllers());

            tenant.SetupStore(app.ApplicationServices);
        }
    }
}
