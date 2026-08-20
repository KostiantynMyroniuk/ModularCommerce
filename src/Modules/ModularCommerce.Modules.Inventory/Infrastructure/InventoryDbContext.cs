using Microsoft.EntityFrameworkCore;
using ModularCommerce.Modules.Inventory.Models;

namespace ModularCommerce.Modules.Inventory.Infrastructure
{
    internal class InventoryDbContext(DbContextOptions<InventoryDbContext> options) : DbContext(options)
    {
        public DbSet<InventoryProduct> InventoryProducts { get; set; }

        public DbSet<InventoryReservation> InventoryReservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(InventoryDbContext).Assembly);
            modelBuilder.HasDefaultSchema("inventory");
        }
    }
}
