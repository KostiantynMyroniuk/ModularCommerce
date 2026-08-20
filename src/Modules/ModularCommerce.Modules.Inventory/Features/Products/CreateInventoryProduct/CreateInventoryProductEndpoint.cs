using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using ModularCommerce.Shared.Enpoints;

namespace ModularCommerce.Modules.Inventory.Features.Products.CreateInventoryProduct
{
    internal class CreateInventoryProductEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/inventory/products", async (
                CreateInventoryProductCommand command,
                ISender sender,
                CancellationToken ct) =>
            {
                var response = await sender.Send(command, ct);

                return response.IsSuccess
                    ? Results.Created($"/api/inventory/products/{response.Value}", response.Value)
                    : Results.Conflict(response.ErrorMessage);
            })
            .WithTags("Inventory");
        }
    }
}
