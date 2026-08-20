using MediatR;
using Microsoft.EntityFrameworkCore;
using ModularCommerce.Modules.Inventory.Dtos;
using ModularCommerce.Modules.Inventory.Infrastructure;
using ModularCommerce.Shared.Results;

namespace ModularCommerce.Modules.Inventory.Features.Products.GetInventoryProducts
{
    internal record GetInventoryProductsQuery() : IRequest<Result<IReadOnlyCollection<GetInventoryProductDto>>>;

    internal class GetInventoryProductsQueryHandler : IRequestHandler<GetInventoryProductsQuery, Result<IReadOnlyCollection<GetInventoryProductDto>>>
    {
        private readonly InventoryDbContext _context;

        public GetInventoryProductsQueryHandler(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<Result<IReadOnlyCollection<GetInventoryProductDto>>> Handle(GetInventoryProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _context.InventoryProducts
                .AsNoTracking()
                .Select(product => new GetInventoryProductDto(
                    product.Id,
                    product.Name,
                    product.Description,
                    product.Sku,
                    product.QuantityOnHand,
                    product.ReservedQuantity,
                    product.AvailableQuantity))
                .ToListAsync(cancellationToken);

            return Result<IReadOnlyCollection<GetInventoryProductDto>>.Success(products);
        }
    }
}
