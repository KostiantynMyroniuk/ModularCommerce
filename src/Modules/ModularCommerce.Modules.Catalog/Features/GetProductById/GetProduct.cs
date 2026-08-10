using MediatR;
using ModularCommerce.Shared.Results;
using ModularCommerce.Modules.Catalog.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using ModularCommerce.Modules.Catalog.Models;
using ModularCommerce.Modules.Catalog.Dtos;

namespace ModularCommerce.Modules.Catalog.Features.GetProductById
{
    internal record GetProductQuery(Guid ProductId) : IRequest<Result<GetProductDto>>;

    internal class GetProductQueryHandler : IRequestHandler<GetProductQuery, Result<GetProductDto>>
    {
        private readonly CatalogDbContext _context;

        public GetProductQueryHandler(CatalogDbContext context)
        {
            _context = context;
        }

        public async Task<Result<GetProductDto>> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var product = await _context.Products
                .FindAsync([request.ProductId] , cancellationToken);

            if (product is null)
                return Result<GetProductDto>.Fail($"Product {request.ProductId} not found");

            return Result<GetProductDto>.Success(
                new GetProductDto(
                    product.Name,
                    product.Description,
                    product.Sku,
                    product.Price));
        }
    }
}
