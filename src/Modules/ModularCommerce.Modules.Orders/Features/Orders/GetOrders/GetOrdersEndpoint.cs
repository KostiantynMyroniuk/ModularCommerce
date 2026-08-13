using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ModularCommerce.Modules.Orders.Dtos;
using ModularCommerce.Modules.Orders.Models;
using ModularCommerce.Shared.Enpoints;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularCommerce.Modules.Orders.Features.Orders.GetOrders
{
    internal class GetOrdersEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/orders", async (
                ISender sender,
                CancellationToken ct,
                [FromQuery] int pageNumber = 1,
                [FromQuery] int pageSize = 10) =>
            {
                var orders = await sender.Send(new GetOrdersQuery(pageNumber, pageSize), ct);

                return Results.Ok(orders);
            })
            .WithTags("Orders");
        }
    }
}
