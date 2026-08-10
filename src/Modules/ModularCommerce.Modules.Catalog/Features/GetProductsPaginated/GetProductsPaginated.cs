using MediatR;
using Microsoft.EntityFrameworkCore;
using ModularCommerce.Modules.Catalog.Dtos;
using ModularCommerce.Modules.Catalog.Features.GetProductById;
using ModularCommerce.Modules.Catalog.Infrastructure;
using ModularCommerce.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularCommerce.Modules.Catalog.Features.GetProductsPaginated
{
    internal record GetProductsPaginatedQuery(
        int PageNumber, 
        int PageSize) : IRequest<PaginatedList<GetProductDto>>;

    internal class GetProductsPaginatedQueryHandler : IRequestHandler<GetProductsPaginatedQuery, PaginatedList<GetProductDto>>
    {
        private readonly CatalogDbContext _context;

        public GetProductsPaginatedQueryHandler(CatalogDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedList<GetProductDto>> Handle(GetProductsPaginatedQuery request, CancellationToken cancellationToken)
        {
            var pageNumber = request.PageNumber;
            var pageSize = request.PageSize;

            var productsQuery = _context.Products.AsNoTracking();

            var totalCount = await productsQuery.CountAsync(cancellationToken);

            var productsPaginated = await productsQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new GetProductDto(
                    p.Name, 
                    p.Description,
                    p.Sku, 
                    p.Price))
                .ToListAsync(cancellationToken);

            return new PaginatedList<GetProductDto>(productsPaginated, pageNumber, pageSize, totalCount);
        }
    }
}
