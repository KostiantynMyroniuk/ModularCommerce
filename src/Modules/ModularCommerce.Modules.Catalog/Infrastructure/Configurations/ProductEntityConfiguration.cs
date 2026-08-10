using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModularCommerce.Modules.Catalog.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularCommerce.Modules.Catalog.Infrastructure.Configurations
{
    internal class ProductEntityConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Description)
                .IsRequired(false)
                .HasMaxLength(512);

            builder.Property(p => p.Sku)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.Price)
                .HasPrecision(18, 2);
        }
    }
}
