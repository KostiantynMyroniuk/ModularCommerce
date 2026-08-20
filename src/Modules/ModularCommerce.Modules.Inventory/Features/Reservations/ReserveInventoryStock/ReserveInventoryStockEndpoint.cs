using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ModularCommerce.Shared.Enpoints;

namespace ModularCommerce.Modules.Inventory.Features.Reservations.ReserveInventoryStock
{
    internal class ReserveInventoryStockEndpoint : IEndpoint
    {
        internal record ReserveInventoryStockCommandDto(int Quantity);

        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/inventory/products/{productId:guid}/reservations", async (
                [FromHeader(Name = "X-Request-Id")] Guid requestId,
                Guid productId,
                ReserveInventoryStockCommandDto commandDto,
                ISender sender,
                CancellationToken ct) =>
            {
                var command = new ReserveInventoryStockCommand(requestId, productId, commandDto.Quantity);
                var response = await sender.Send(command, ct);

                return response.IsSuccess
                    ? Results.Created($"/api/inventory/reservations/{response.Value}", response.Value)
                    : Results.BadRequest(response.ErrorMessage);
            })
            .WithTags("Inventory");
        }
    }
}
