using System;
using System.Collections.Generic;
using System.Text;

namespace ModularCommerce.Modules.Catalog.Dtos
{
    internal record GetProductDto(string Name, string? Description, string Sku, decimal Price);
}
