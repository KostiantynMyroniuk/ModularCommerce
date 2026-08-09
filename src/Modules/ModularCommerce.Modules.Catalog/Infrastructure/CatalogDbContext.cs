using Microsoft.EntityFrameworkCore;
using ModularCommerce.Modules.Catalog.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ModularCommerce.Modules.Catalog.Infrastructure
{
    internal class CatalogDbContext(DbContextOptions<CatalogDbContext> options) : DbContext(options)
    {
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogDbContext).Assembly);

            modelBuilder.HasDefaultSchema("catalog");
        }
    }
}
