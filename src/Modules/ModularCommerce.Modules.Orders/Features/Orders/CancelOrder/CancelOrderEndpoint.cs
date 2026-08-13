using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ModularCommerce.Shared.Enpoints;
using System;

namespace ModularCommerce.Modules.Orders.Features.Orders.CancelOrder
{
    internal class CancelOrderEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/orders/{orderId}/cancel", async (
                [FromRoute] Guid orderId,
                ISender sender,
                CancellationToken ct) =>
            {
                var response = await sender.Send(new CancelOrderCommand(orderId), ct);

                if (response.IsSuccess)
                {
                    return Results.NoContent();
                }

                return response.ErrorMessage?.Contains("not found", StringComparison.OrdinalIgnoreCase) == true
                    ? Results.NotFound(response.ErrorMessage)
                    : Results.BadRequest(response.ErrorMessage);
            })
            .WithTags("Orders");
        }
    }
}
