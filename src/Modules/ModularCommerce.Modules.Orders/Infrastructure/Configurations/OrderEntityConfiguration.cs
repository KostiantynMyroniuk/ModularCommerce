using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModularCommerce.Modules.Orders.Models;

namespace ModularCommerce.Modules.Orders.Infrastructure.Configurations
{
    internal class OrderEntityConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(order => order.RequestId)
                .IsRequired();

            builder.HasIndex(order => order.RequestId)
                .IsUnique();

            builder.Property(order => order.OrderNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(order => order.TotalAmount)
                .HasPrecision(18, 2);

            builder.Property(order => order.AddressLine1)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(order => order.AddressLine2)
                .HasMaxLength(200);

            builder.Property(order => order.City)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(order => order.Country)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(order => order.Region)
                .HasMaxLength(100);

            builder.Property(order => order.PostalCode)
                .HasMaxLength(20);

            builder.Property(order => order.Status)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(order => order.CreatedAt)
                .IsRequired();

            builder.HasMany(order => order.Items)
                .WithOne()
                .HasForeignKey(item => item.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
