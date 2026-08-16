using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ModularCommerce.Shared.Enpoints;
using ModularCommerce.Shared.Results;

namespace ModularCommerce.Modules.Orders.Features.Orders.CreateOrder
{
    internal class CreateOrderEndpoint : IEndpoint
    {
        internal record CreateOrderCommandDto(
            AddressDto Address,
            IReadOnlyCollection<OrderItemDto> Items);

        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/orders", async (
                [FromHeader(Name = "X-Request-Id")] Guid requestId,
                CreateOrderCommandDto commandDto,
                ISender sender,
                CancellationToken ct) =>
            {
                var command = new CreateOrderCommand(requestId, commandDto.Address, commandDto.Items);

                var response = await sender.Send(command, ct);

                return response.IsSuccess
                    ? Results.Created($"/api/orders/{response.Value}", response.Value)
                    : Results.BadRequest(response.ErrorMessage);
            })
            .WithTags("Orders");
        }
    }
}
