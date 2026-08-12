using Microsoft.EntityFrameworkCore;
using ModularCommerce.Modules.Catalog.Abstractions;
using ModularCommerce.Modules.Catalog.Infrastructure;
using ModularCommerce.Shared.Results;

namespace ModularCommerce.Modules.Catalog.Infrastructure.Services
{
    internal sealed class ProductCatalogReader : IProductCatalogReader
    {
        private readonly CatalogDbContext _context;

        public ProductCatalogReader(CatalogDbContext context)
        {
            _context = context;
        }

        public async Task<Result<IReadOnlyCollection<CatalogProductSnapshot>>> GetProductsAsync(
            IReadOnlyCollection<Guid> productIds,
            CancellationToken cancellationToken = default)
        {
            var uniqueIds = productIds.Distinct().ToArray();

            var products = await _context.Products
                .AsNoTracking()
                .Where(product => uniqueIds.Contains(product.Id))
                .Select(product => new CatalogProductSnapshot(
                    product.Id,
                    product.Name,
                    product.Description,
                    product.Sku,
                    product.Price))
                .ToListAsync(cancellationToken);

            if (products.Count != uniqueIds.Length)
            {
                var foundIds = products.Select(product => product.ProductId).ToHashSet();
                var missingIds = uniqueIds.Where(id => !foundIds.Contains(id));

                return Result<IReadOnlyCollection<CatalogProductSnapshot>>.Fail(
                    $"Products not found: {string.Join(", ", missingIds)}");
            }

            return Result<IReadOnlyCollection<CatalogProductSnapshot>>.Success(products);
        }
    }
}
