using ModularCommerce.Shared.Results;

namespace ModularCommerce.Modules.Catalog.Abstractions
{
    public interface IProductCatalogReader
    {
        Task<Result<IReadOnlyCollection<CatalogProductSnapshot>>> GetProductsAsync(
            IReadOnlyCollection<Guid> productIds,
            CancellationToken cancellationToken = default);
    }
}
