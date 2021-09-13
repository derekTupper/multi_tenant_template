using DataLayer.Entities.Common;
using Finbuckle.MultiTenant;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using Common.Models;
using System.Threading.Tasks;

namespace DataLayer.Entities.Tenant
{
    public partial class TenantContext : MultiTenantDbContext
    {
        public TenantContext(ITenantInfo tenantInfo) : base(tenantInfo) { }
        public TenantContext(ITenantInfo tenantInfo, DbContextOptions<TenantContext> options) :
            base(tenantInfo, options)
        { }
        
        /* Insert Tenant Model References Here */


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(TenantInfo.ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }

        public override int SaveChanges()
        {
            UpdateActiveStatuses();
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (
                    e.State == EntityState.Added
                    || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((BaseEntity)entityEntry.Entity).UpdatedDate = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseEntity)entityEntry.Entity).CreatedDate = DateTime.Now;
                }
            }
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            UpdateActiveStatuses();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void UpdateActiveStatuses()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.CurrentValues["Active"] = false;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.CurrentValues["Active"] = false;
                        break;
                }
            }
        }
    }
}
