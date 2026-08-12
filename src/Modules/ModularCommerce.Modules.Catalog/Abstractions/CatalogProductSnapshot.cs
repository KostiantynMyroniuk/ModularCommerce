namespace ModularCommerce.Modules.Catalog.Abstractions
{
    public sealed record CatalogProductSnapshot(
        Guid ProductId,
        string Name,
        string? Description,
        string Sku,
        decimal Price);
}
