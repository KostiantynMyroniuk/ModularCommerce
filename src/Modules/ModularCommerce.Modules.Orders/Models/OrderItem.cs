using System;

namespace ModularCommerce.Modules.Orders.Models
{
    internal class OrderItem
    {
        public Guid Id { get; private set; }
        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; } = default!;
        public string? ProductDescription { get; private set; }
        public string Sku { get; private set; } = default!;
        public decimal UnitPrice { get; private set; }
        public int Quantity { get; private set; }
        public decimal LineTotal { get; private set; }

        private OrderItem() { }

        public static OrderItem Create(
            Guid productId,
            string productName,
            string? productDescription,
            string sku,
            decimal unitPrice,
            int quantity)
        {
            return new OrderItem
            {
                Id = Guid.CreateVersion7(),
                ProductId = productId,
                ProductName = productName,
                ProductDescription = productDescription,
                Sku = sku,
                UnitPrice = unitPrice,
                Quantity = quantity,
                LineTotal = unitPrice * quantity
            };
        }
    }
}
