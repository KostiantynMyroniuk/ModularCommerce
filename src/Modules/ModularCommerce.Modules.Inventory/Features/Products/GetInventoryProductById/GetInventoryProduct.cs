using MediatR;
using Microsoft.EntityFrameworkCore;
using ModularCommerce.Modules.Inventory.Dtos;
using ModularCommerce.Modules.Inventory.Infrastructure;
using ModularCommerce.Shared.Results;

namespace ModularCommerce.Modules.Inventory.Features.Products.GetInventoryProductById
{
    internal record GetInventoryProductQuery(Guid ProductId) : IRequest<Result<GetInventoryProductDto>>;

    internal class GetInventoryProductQueryHandler : IRequestHandler<GetInventoryProductQuery, Result<GetInventoryProductDto>>
    {
        private readonly InventoryDbContext _context;

        public GetInventoryProductQueryHandler(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<Result<GetInventoryProductDto>> Handle(GetInventoryProductQuery request, CancellationToken cancellationToken)
        {
            var product = await _context.InventoryProducts
                .FindAsync([request.ProductId], cancellationToken);

            if (product is null)
            {
                return Result<GetInventoryProductDto>.Fail($"Inventory product {request.ProductId} not found");
            }

            return Result<GetInventoryProductDto>.Success(new GetInventoryProductDto(
                product.Id,
                product.Name,
                product.Description,
                product.Sku,
                product.QuantityOnHand,
                product.ReservedQuantity,
                product.AvailableQuantity));
        }
    }
}
