using Microsoft.EntityFrameworkCore;

namespace DataLayer.Entities.Platform
{
    public partial class PlatformConfigurations
    {
        protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");


            modelBuilder.Entity<Tenant>(entity =>
            {
                entity.ToTable("Tenant");
 
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");
            });
            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
