using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using ModularCommerce.Shared.Enpoints;

namespace ModularCommerce.Modules.Inventory.Features.Products.GetInventoryProducts
{
    internal class GetInventoryProductsEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/inventory/products", async (
                ISender sender,
                CancellationToken ct) =>
            {
                var response = await sender.Send(new GetInventoryProductsQuery(), ct);

                return response.IsSuccess
                    ? Results.Ok(response.Value)
                    : Results.BadRequest(response.ErrorMessage);
            })
            .WithTags("Inventory");
        }
    }
}
