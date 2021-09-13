using DataLayer.Entities.Common;
using DataLayer.Entities.Platform;
using Finbuckle.MultiTenant.Stores;
using Microsoft.EntityFrameworkCore;
using Common.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DataLayer
{
    public partial class PlatformContext : EFCoreStoreDbContext<TenantModel>
    {
        public PlatformContext(DbContextOptions<PlatformContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Tenant> Tenants { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer();
            }
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
                    ((BaseEntity)entityEntry.Entity).Active = true;

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

