using DataLayer.Entities;
using Finbuckle.MultiTenant.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities.Tenant
{
    public partial class TenantConfigurations
    {
        protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.ConfigureMultiTenant();

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
