using MediatR;
using Microsoft.EntityFrameworkCore;
using ModularCommerce.Modules.Orders.Dtos;
using ModularCommerce.Modules.Orders.Infrastructure;
using ModularCommerce.Modules.Orders.Models;
using ModularCommerce.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularCommerce.Modules.Orders.Features.Orders.GetOrders
{
    internal record GetOrdersQuery(
        int PageNumber,
        int PageSize
    ) : IRequest<PaginatedList<GetAllOrdersDto>>;

    internal class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, PaginatedList<GetAllOrdersDto>>
    {
        private readonly OrdersDbContext _context;

        public GetOrdersQueryHandler(OrdersDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedList<GetAllOrdersDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            var pageNumber = request.PageNumber;
            var pageSize = request.PageSize;

            var ordersQuery = _context.Orders
                .AsNoTracking();

            var totalCount = await ordersQuery.CountAsync();

            var ordersPaginatedDto = await ordersQuery
                .OrderBy(o => o.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(o => new GetAllOrdersDto(
                    o.Id,
                    o.OrderNumber,
                    o.Status,
                    o.TotalAmount))
                .ToListAsync();

            return new PaginatedList<GetAllOrdersDto>(ordersPaginatedDto, pageNumber, pageSize, totalCount);
        }
    }
}
