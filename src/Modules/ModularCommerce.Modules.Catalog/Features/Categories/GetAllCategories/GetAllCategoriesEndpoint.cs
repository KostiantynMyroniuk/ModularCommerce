using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ModularCommerce.Shared.Enpoints;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularCommerce.Modules.Catalog.Features.Categories.GetAllCategories
{
    internal class GetAllCategoriesEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/categories", async (
                [FromQuery] Guid? parentId,
                ISender sender,
                CancellationToken ct) =>
            {
                var categories = await sender.Send(new GetAllCategoriesQuery(parentId), ct);

                return Results.Ok(categories);
            })
            .WithTags("Categories");
        }
    }
}
