using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModularCommerce.Modules.Orders.Models;

namespace ModularCommerce.Modules.Orders.Infrastructure.Configurations
{
    internal class OrderItemEntityConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.Property(item => item.OrderId)
                .IsRequired();

            builder.HasIndex(item => item.OrderId);

            builder.Property(item => item.ProductId)
                .IsRequired();

            builder.Property(item => item.ProductName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(item => item.ProductDescription)
                .HasMaxLength(512);

            builder.Property(item => item.Sku)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(item => item.UnitPrice)
                .HasPrecision(18, 2);

            builder.Property(item => item.Quantity)
                .IsRequired();

            builder.Property(item => item.LineTotal)
                .HasPrecision(18, 2);
        }
    }
}
