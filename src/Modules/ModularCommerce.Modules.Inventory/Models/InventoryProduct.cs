using System;

namespace ModularCommerce.Modules.Inventory.Models
{
    internal class InventoryProduct
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = default!;
        public string? Description { get; private set; }
        public string Sku { get; private set; } = default!;
        public int QuantityOnHand { get; private set; }
        public int ReservedQuantity { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        public int AvailableQuantity => QuantityOnHand - ReservedQuantity;

        private InventoryProduct() { }

        public static InventoryProduct Create(
            string name,
            string? description,
            string sku,
            int quantityOnHand)
        {
            return new InventoryProduct
            {
                Id = Guid.CreateVersion7(),
                Name = name,
                Description = description,
                Sku = sku.ToUpperInvariant(),
                QuantityOnHand = quantityOnHand,
                ReservedQuantity = 0,
                CreatedAt = DateTime.UtcNow
            };
        }

        public void AdjustQuantity(int quantityDelta)
        {
            QuantityOnHand += quantityDelta;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Reserve(int quantity)
        {
            ReservedQuantity += quantity;
            UpdatedAt = DateTime.UtcNow;
        }

        public void CancelReservation(int quantity)
        {
            ReservedQuantity -= quantity;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
