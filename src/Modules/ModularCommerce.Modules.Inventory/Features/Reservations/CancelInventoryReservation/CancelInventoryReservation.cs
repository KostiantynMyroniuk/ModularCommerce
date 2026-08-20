using MediatR;
using Microsoft.EntityFrameworkCore;
using ModularCommerce.Modules.Inventory.Infrastructure;
using ModularCommerce.Modules.Inventory.Models;
using ModularCommerce.Shared.Results;

namespace ModularCommerce.Modules.Inventory.Features.Reservations.CancelInventoryReservation
{
    internal record CancelInventoryReservationCommand(Guid ReservationId) : IRequest<Result>;

    internal class CancelInventoryReservationCommandHandler : IRequestHandler<CancelInventoryReservationCommand, Result>
    {
        private readonly InventoryDbContext _context;

        public CancelInventoryReservationCommandHandler(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(CancelInventoryReservationCommand request, CancellationToken cancellationToken)
        {
            var reservation = await _context.InventoryReservations
                .FirstOrDefaultAsync(x => x.Id == request.ReservationId, cancellationToken);

            if (reservation is null)
            {
                return Result.Fail($"Inventory reservation {request.ReservationId} not found");
            }

            if (reservation.Status == ReservationStatus.Cancelled)
            {
                return Result.Success();
            }

            var product = await _context.InventoryProducts
                .FirstOrDefaultAsync(x => x.Id == reservation.InventoryProductId, cancellationToken);

            if (product is null)
            {
                return Result.Fail($"Inventory product {reservation.InventoryProductId} not found");
            }

            product.CancelReservation(reservation.Quantity);
            reservation.Cancel();

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
