using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using ModularCommerce.Shared.Enpoints;

namespace ModularCommerce.Modules.Orders.Features.Orders.CreateOrder
{
    internal class CreateOrderEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/orders", async (
                CreateOrderCommand command,
                ISender sender,
                CancellationToken ct) =>
            {
                var response = await sender.Send(command, ct);

                return response.IsSuccess
                    ? Results.Created($"/api/orders/{response.Value}", response.Value)
                    : Results.BadRequest(response.ErrorMessage);
            })
            .WithTags("Orders");
        }
    }
}
