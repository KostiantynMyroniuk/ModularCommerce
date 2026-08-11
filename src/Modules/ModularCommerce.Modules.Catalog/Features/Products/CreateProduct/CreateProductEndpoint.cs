using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using ModularCommerce.Shared.Enpoints;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularCommerce.Modules.Catalog.Features.Products.CreateProduct
{
    internal class CreateProductEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/catalog", async (
                CreateProductCommand command,
                ISender sender, 
                CancellationToken ct) =>
            {
                var response = await sender.Send(command, ct);

                return Results.Created($"/api/catalog/{response.Value}", response.Value);
            });
        }
    }
}
