using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ModularCommerce.Shared.Enpoints;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularCommerce.Modules.Catalog.Features.DeleteProduct
{
    internal class DeleteProductEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("/api/catalog/{productId}", async (
                [FromRoute] Guid productId,
                ISender sender,
                CancellationToken ct) =>
            {
                var response = await sender.Send(new DeleteProductCommand(productId), ct);

                return response.IsSuccess 
                    ? Results.NoContent() 
                    : Results.NotFound(response.ErrorMessage);
            });
        }
    }
}
