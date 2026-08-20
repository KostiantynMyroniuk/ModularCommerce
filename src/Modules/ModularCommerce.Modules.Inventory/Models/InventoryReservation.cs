using System;

namespace ModularCommerce.Modules.Inventory.Models
{
    internal class InventoryReservation
    {
        public Guid Id { get; private set; }
        public Guid RequestId { get; private set; }
        public Guid InventoryProductId { get; private set; }
        public int Quantity { get; private set; }
        public ReservationStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? CancelledAt { get; private set; }

        private InventoryReservation() { }

        public static InventoryReservation Create(
            Guid requestId,
            Guid inventoryProductId,
            int quantity)
        {
            return new InventoryReservation
            {
                Id = Guid.CreateVersion7(),
                RequestId = requestId,
                InventoryProductId = inventoryProductId,
                Quantity = quantity,
                Status = ReservationStatus.Reserved,
                CreatedAt = DateTime.UtcNow
            };
        }

        public void Cancel()
        {
            Status = ReservationStatus.Cancelled;
            CancelledAt = DateTime.UtcNow;
        }
    }
}
