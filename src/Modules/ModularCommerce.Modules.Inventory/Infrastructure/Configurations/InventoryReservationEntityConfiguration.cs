using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModularCommerce.Modules.Inventory.Models;

namespace ModularCommerce.Modules.Inventory.Infrastructure.Configurations
{
    internal class InventoryReservationEntityConfiguration : IEntityTypeConfiguration<InventoryReservation>
    {
        public void Configure(EntityTypeBuilder<InventoryReservation> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.RequestId)
                .IsRequired();

            builder.Property(x => x.InventoryProductId)
                .IsRequired();

            builder.Property(x => x.Quantity)
                .IsRequired();

            builder.Property(x => x.Status)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.CancelledAt);

            builder.HasIndex(x => new { x.RequestId, x.InventoryProductId })
                .IsUnique();

            builder.HasOne<InventoryProduct>()
                .WithMany()
                .HasForeignKey(x => x.InventoryProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
