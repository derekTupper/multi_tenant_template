using Common.Models;
using DataLayer.Entities.Tenant;
using Finbuckle.MultiTenant;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.IO;

namespace CMS.Api
{
    public class TenantContextFactory : IDesignTimeDbContextFactory<TenantContext>
    { 
        public TenantContext CreateDbContext(string [] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<TenantContext>();
            var connectionString = configuration.GetConnectionString("BaseTenantContext");

            builder.UseSqlServer(connectionString);

            TenantInfo tenantInfo = new TenantInfo
            {
                ConnectionString = connectionString
            };
            return new TenantContext(tenantInfo);
        }
    }
}
