using MediatR;
using Microsoft.EntityFrameworkCore;
using ModularCommerce.Modules.Inventory.Dtos;
using ModularCommerce.Modules.Inventory.Infrastructure;
using ModularCommerce.Shared.Results;

namespace ModularCommerce.Modules.Inventory.Features.Products.AdjustInventoryQuantity
{
    internal record AdjustInventoryQuantityCommand(
        Guid ProductId,
        int QuantityDelta) : IRequest<Result<Guid>>;

    internal class AdjustInventoryQuantityCommandHandler : IRequestHandler<AdjustInventoryQuantityCommand, Result<Guid>>
    {
        private readonly InventoryDbContext _context;

        public AdjustInventoryQuantityCommandHandler(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<Result<Guid>> Handle(AdjustInventoryQuantityCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.InventoryProducts
                .FirstOrDefaultAsync(x => x.Id == request.ProductId, cancellationToken);

            if (product is null)
            {
                return Result<Guid>.Fail($"Inventory product {request.ProductId} not found");
            }

            var newQuantity = product.QuantityOnHand + request.QuantityDelta;
            if (newQuantity < 0)
            {
                return Result<Guid>.Fail("Quantity on hand cannot be negative.");
            }

            if (newQuantity < product.ReservedQuantity)
            {
                return Result<Guid>.Fail("Quantity on hand cannot be less than reserved quantity.");
            }

            product.AdjustQuantity(request.QuantityDelta);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(product.Id);
        }
    }
}
