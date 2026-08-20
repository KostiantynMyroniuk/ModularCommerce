using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using ModularCommerce.Shared.Enpoints;

namespace ModularCommerce.Modules.Inventory.Features.Products.AdjustInventoryQuantity
{
    internal class AdjustInventoryQuantityEndpoint : IEndpoint
    {
        internal record AdjustInventoryQuantityCommandDto(int QuantityDelta);

        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPatch("/api/inventory/products/{productId:guid}/quantity", async (
                Guid productId,
                AdjustInventoryQuantityCommandDto commandDto,
                ISender sender,
                CancellationToken ct) =>
            {
                var command = new AdjustInventoryQuantityCommand(productId, commandDto.QuantityDelta);
                var response = await sender.Send(command, ct);

                return response.IsSuccess
                    ? Results.Ok(response.Value)
                    : Results.BadRequest(response.ErrorMessage);
            })
            .WithTags("Inventory");
        }
    }
}
