using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ModularCommerce.Modules.Inventory.Infrastructure;
using ModularCommerce.Modules.Inventory.Models;
using ModularCommerce.Shared.Results;

namespace ModularCommerce.Modules.Inventory.Features.Reservations.ReserveInventoryStock
{
    internal record ReserveInventoryStockCommand(
        Guid RequestId,
        Guid ProductId,
        int Quantity) : IRequest<Result<Guid>>;

    internal class ReserveInventoryStockCommandHandler : IRequestHandler<ReserveInventoryStockCommand, Result<Guid>>
    {
        private readonly InventoryDbContext _context;
        private readonly ILogger<ReserveInventoryStockCommandHandler> _logger;

        public ReserveInventoryStockCommandHandler(
            InventoryDbContext context,
            ILogger<ReserveInventoryStockCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<Guid>> Handle(ReserveInventoryStockCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.InventoryProducts
                .FirstOrDefaultAsync(x => x.Id == request.ProductId, cancellationToken);

            if (product is null)
            {
                _logger.LogWarning("Inventory product {InventoryProductId} not found for RequestId {RequestId}", request.ProductId, request.RequestId);
                return Result<Guid>.Fail($"Inventory product {request.ProductId} not found");
            }

            if (request.Quantity > product.AvailableQuantity)
            {
                _logger.LogWarning(
                    "Not enough stock to reserve InventoryProductId {InventoryProductId} for RequestId {RequestId}. Requested {Quantity}, available {AvailableQuantity}",
                    request.ProductId,
                    request.RequestId,
                    request.Quantity,
                    product.AvailableQuantity);

                return Result<Guid>.Fail("Not enough available stock to reserve.");
            }

            var existingReservation = await _context.InventoryReservations
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.RequestId == request.RequestId && x.InventoryProductId == request.ProductId, cancellationToken);

            if (existingReservation is not null)
            {
                _logger.LogInformation(
                    "Inventory reservation {ReservationId} already exists for RequestId {RequestId} and InventoryProductId {InventoryProductId}",
                    existingReservation.Id,
                    request.RequestId,
                    request.ProductId);

                return Result<Guid>.Success(existingReservation.Id);
            }

            var reservation = InventoryReservation.Create(request.RequestId, request.ProductId, request.Quantity);

            try
            {
                product.Reserve(request.Quantity);
                _context.InventoryReservations.Add(reservation);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation(
                    "Inventory reservation {ReservationId} created for RequestId {RequestId} and InventoryProductId {InventoryProductId}",
                    reservation.Id,
                    request.RequestId,
                    request.ProductId);

                return Result<Guid>.Success(reservation.Id);
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException { Number: 2627 or 2601 })
            {
                _context.ChangeTracker.Clear();

                var duplicateReservation = await _context.InventoryReservations
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.RequestId == request.RequestId && x.InventoryProductId == request.ProductId, cancellationToken);

                if (duplicateReservation is not null)
                {
                    _logger.LogInformation(
                        "Inventory reservation {ReservationId} already exists after unique constraint conflict for RequestId {RequestId} and InventoryProductId {InventoryProductId}",
                        duplicateReservation.Id,
                        request.RequestId,
                        request.ProductId);

                    return Result<Guid>.Success(duplicateReservation.Id);
                }

                _logger.LogWarning(
                    ex,
                    "Failed to reserve inventory stock for RequestId {RequestId} and InventoryProductId {InventoryProductId}",
                    request.RequestId,
                    request.ProductId);

                return Result<Guid>.Fail("Failed to reserve inventory stock.");
            }
        }
    }
}
