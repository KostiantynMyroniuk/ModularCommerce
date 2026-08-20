using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModularCommerce.Modules.Inventory.Models;

namespace ModularCommerce.Modules.Inventory.Infrastructure.Configurations
{
    internal class InventoryProductEntityConfiguration : IEntityTypeConfiguration<InventoryProduct>
    {
        public void Configure(EntityTypeBuilder<InventoryProduct> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Description)
                .HasMaxLength(1000);

            builder.Property(x => x.Sku)
                .IsRequired()
                .HasMaxLength(64);

            builder.Property(x => x.QuantityOnHand)
                .IsRequired();

            builder.Property(x => x.ReservedQuantity)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.UpdatedAt);

            builder.HasIndex(x => x.Sku)
                .IsUnique();
        }
    }
}
