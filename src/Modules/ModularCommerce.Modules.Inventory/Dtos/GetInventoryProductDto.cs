using System;

namespace ModularCommerce.Modules.Inventory.Dtos
{
    internal record GetInventoryProductDto(
        Guid Id,
        string Name,
        string? Description,
        string Sku,
        int QuantityOnHand,
        int ReservedQuantity,
        int AvailableQuantity);
}
