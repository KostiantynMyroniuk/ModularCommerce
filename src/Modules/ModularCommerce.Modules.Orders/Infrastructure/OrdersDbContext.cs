using Microsoft.EntityFrameworkCore;
using ModularCommerce.Modules.Orders.Models;

namespace ModularCommerce.Modules.Orders.Infrastructure
{
    internal class OrdersDbContext(DbContextOptions<OrdersDbContext> options) : DbContext(options)
    {
        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrdersDbContext).Assembly);
            modelBuilder.HasDefaultSchema("orders");
        }
    }
}
