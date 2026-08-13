using System;
using System.Collections.Generic;
using System.Linq;

namespace ModularCommerce.Modules.Orders.Models
{
    internal class Order
    {
        private readonly List<OrderItem> _items = [];

        public Guid Id { get; private set; }
        public Guid RequestId { get; private set; }
        public string OrderNumber { get; private set; } = default!;
        public decimal TotalAmount { get; private set; }
        public string AddressLine1 { get; private set; } = default!;
        public string? AddressLine2 { get; private set; }
        public string City { get; private set; } = default!;
        public string Country { get; private set; } = default!;
        public string? Region { get; private set; }
        public string? PostalCode { get; private set; }
        public OrderStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public ICollection<OrderItem> Items => _items;
        public bool CanCancel => Status is OrderStatus.Pending or OrderStatus.Confirmed or OrderStatus.Processing;
        public bool IsCancelled => Status == OrderStatus.Cancelled;

        public void Cancel()
        {
            Status = OrderStatus.Cancelled;
        }

        private Order() { }

        public static Order Create(
            Guid requestId,
            string addressLine1,
            string? addressLine2,
            string city,
            string country,
            string? region,
            string? postalCode,
            IEnumerable<OrderItem> items)
        {
            var orderItems = items.ToList();
            var order = new Order
            {
                Id = Guid.CreateVersion7(),
                RequestId = requestId,
                OrderNumber = GenerateOrderNumber(),
                TotalAmount = orderItems.Sum(item => item.LineTotal),
                AddressLine1 = addressLine1,
                AddressLine2 = addressLine2,
                City = city,
                Country = country,
                Region = region,
                PostalCode = postalCode,
                Status = OrderStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            order._items.AddRange(orderItems);
            return order;
        }

        private static string GenerateOrderNumber()
        {
            return $"ORD-{Guid.CreateVersion7():N}";
        }
    }
}
