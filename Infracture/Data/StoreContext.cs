using System.Linq;
using System.Reflection;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infracture.Data
{
    public class StoreContext : DbContext
    {
        // The DBContextOptions will give us the option to create connection string
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductBrand> ProductBrands { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }

        // Override OnModelCreating method which create migrations and tell it to look for custom configuration inside the project
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // base is the class that we're deriving from; In this case it is DbContext
            base.OnModelCreating(modelBuilder);
            // Explicitly telling the model builder to look for configuration inside the project
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Sqlite doesn't support decimal; work around is to convert it to double
            // Check the provider name and convert to double

            if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
            {
                foreach (var entityType in modelBuilder.Model.GetEntityTypes())
                {
                    var properties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == typeof(decimal));

                    foreach (var property in properties)
                    {
                        modelBuilder.Entity(entityType.Name).Property(property.Name).HasConversion<double>();
                    }
                }
            }

        }
    }
}