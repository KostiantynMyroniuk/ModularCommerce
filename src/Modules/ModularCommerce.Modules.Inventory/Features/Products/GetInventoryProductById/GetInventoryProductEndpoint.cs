using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using ModularCommerce.Shared.Enpoints;

namespace ModularCommerce.Modules.Inventory.Features.Products.GetInventoryProductById
{
    internal class GetInventoryProductEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/inventory/products/{productId:guid}", async (
                Guid productId,
                ISender sender,
                CancellationToken ct) =>
            {
                var response = await sender.Send(new GetInventoryProductQuery(productId), ct);

                return response.IsSuccess
                    ? Results.Ok(response.Value)
                    : Results.NotFound(response.ErrorMessage);
            })
            .WithTags("Inventory");
        }
    }
}
