using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using ModularCommerce.Shared.Enpoints;

namespace ModularCommerce.Modules.Inventory.Features.Reservations.CancelInventoryReservation
{
    internal class CancelInventoryReservationEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("/api/inventory/reservations/{reservationId:guid}", async (
                Guid reservationId,
                ISender sender,
                CancellationToken ct) =>
            {
                var response = await sender.Send(new CancelInventoryReservationCommand(reservationId), ct);

                return response.IsSuccess
                    ? Results.NoContent()
                    : Results.BadRequest(response.ErrorMessage);
            })
            .WithTags("Inventory");
        }
    }
}
