using MediatR;
using Microsoft.Extensions.Logging;
using ModularCommerce.Modules.Inventory.Features.Reservations.ReserveInventoryStock;
using ModularCommerce.Shared.Contracts.Orders;

namespace ModularCommerce.Modules.Inventory.Features.Reservations.OrderCreated
{
    internal class OrderCreatedNotificationHandler : INotificationHandler<OrderCreatedNotification>
    {
        private readonly ISender _sender;
        private readonly ILogger<OrderCreatedNotificationHandler> _logger;

        public OrderCreatedNotificationHandler(
            ISender sender,
            ILogger<OrderCreatedNotificationHandler> logger)
        {
            _sender = sender;
            _logger = logger;
        }

        public async Task Handle(OrderCreatedNotification notification, CancellationToken cancellationToken)
        {
            if (notification.Items.Count == 0)
            {
                _logger.LogWarning("OrderCreatedNotification for RequestId {RequestId} does not contain any items", notification.RequestId);
                return;
            }

            _logger.LogInformation(
                "Processing OrderCreatedNotification for RequestId {RequestId} with {ItemCount} items",
                notification.RequestId,
                notification.Items.Count);

            var groupedItems = notification.Items
                .GroupBy(item => item.ProductId)
                .Select(group => new
                {
                    ProductId = group.Key,
                    Quantity = group.Sum(item => item.Quantity)
                });

            foreach (var item in groupedItems)
            {
                var result = await _sender.Send(
                    new ReserveInventoryStockCommand(notification.RequestId, item.ProductId, item.Quantity),
                    cancellationToken);

                if (result.IsFailure)
                {
                    _logger.LogWarning(
                        "Failed to reserve inventory for RequestId {RequestId} and InventoryProductId {InventoryProductId}: {ErrorMessage}",
                        notification.RequestId,
                        item.ProductId,
                        result.ErrorMessage);

                    throw new InvalidOperationException(result.ErrorMessage ?? "Failed to reserve inventory stock.");
                }
            }

            _logger.LogInformation("Finished processing OrderCreatedNotification for RequestId {RequestId}", notification.RequestId);
        }
    }
}
