using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ModularCommerce.Modules.Inventory.Infrastructure;
using ModularCommerce.Modules.Inventory.Models;
using ModularCommerce.Shared.Results;

namespace ModularCommerce.Modules.Inventory.Features.Products.CreateInventoryProduct
{
    internal record CreateInventoryProductCommand(
        string Name,
        string? Description,
        string Sku,
        int InitialQuantity) : IRequest<Result<Guid>>;

    internal class CreateInventoryProductCommandHandler : IRequestHandler<CreateInventoryProductCommand, Result<Guid>>
    {
        private readonly InventoryDbContext _context;
        private readonly ILogger<CreateInventoryProductCommandHandler> _logger;

        public CreateInventoryProductCommandHandler(
            InventoryDbContext context,
            ILogger<CreateInventoryProductCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<Guid>> Handle(CreateInventoryProductCommand request, CancellationToken cancellationToken)
        {
            var product = InventoryProduct.Create(
                request.Name,
                request.Description,
                request.Sku,
                request.InitialQuantity);

            try
            {
                _context.InventoryProducts.Add(product);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Inventory product {InventoryProductId} created successfully", product.Id);
                return Result<Guid>.Success(product.Id);
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException { Number: 2627 or 2601 })
            {
                _logger.LogWarning("Duplicate SKU detected: {Sku} while creating inventory product", product.Sku);

                _context.ChangeTracker.Clear();

                return Result<Guid>.Fail($"Inventory product with SKU: {product.Sku} already exists");
            }
        }
    }
}
