using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ModularCommerce.Shared.Enpoints;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularCommerce.Modules.Catalog.Features.GetProductById
{
    internal class GetProductEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/catalog/{productId}", async (
                [FromRoute] Guid productId,
                ISender sender,
                CancellationToken ct) =>
            {
                var response = await sender.Send(new GetProductQuery(productId), ct);

                return response.IsSuccess 
                    ? Results.Ok(response.Value) 
                    : Results.NotFound(response.ErrorMessage);
            });
        }
    }
}
