using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ModularCommerce.Shared.Enpoints;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularCommerce.Modules.Catalog.Features.Products.GetProductsPaginated
{
    internal class GetProductsPaginatedEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/catalog", async (
                [FromQuery] int pageNumber,
                [FromQuery] int pageSize,
                ISender sender,
                CancellationToken ct) =>
            {
                var response = await sender.Send(new GetProductsPaginatedQuery(pageNumber, pageSize), ct);

                return Results.Ok(response);
            })
            .WithTags("Catalog");

        }
    }
}
