using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infracture.Data
{
    public class StoreContext : DbContext
    {
        // The DB Context Options will give us the option to create connection string
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}