using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ModularCommerce.Modules.Orders.Infrastructure;
using ModularCommerce.Modules.Orders.Models;
using ModularCommerce.Shared.Results;

namespace ModularCommerce.Modules.Orders.Features.Orders.CancelOrder
{
    internal record CancelOrderCommand(Guid OrderId) : IRequest<Result>;

    internal class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, Result>
    {
        private readonly OrdersDbContext _context;
        private readonly ILogger<CancelOrderCommandHandler> _logger;

        public CancelOrderCommandHandler(
            OrdersDbContext context,
            ILogger<CancelOrderCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders.FindAsync([request.OrderId], cancellationToken);
            
            if (order is null)
            {
                _logger.LogWarning("Order {OrderId} not found", request.OrderId);
                return Result.Fail($"Order {request.OrderId} not found");
            }

            if (order.IsCancelled)
            {
                _logger.LogInformation("Order {OrderId} is already cancelled", request.OrderId);
                return Result.Success();
            }

            if (!order.CanCancel)
            {
                _logger.LogWarning(
                    "Order {OrderId} cannot be cancelled from status {Status}",
                    request.OrderId,
                    order.Status);

                return Result.Fail($"Order {request.OrderId} cannot be cancelled in status {order.Status}.");
            }

            order.Cancel();
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Order {OrderId} was cancelled", request.OrderId);
            return Result.Success();
        }
    }
}
