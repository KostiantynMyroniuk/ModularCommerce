using System;
using System.Collections.Generic;
using System.Text;

namespace ModularCommerce.Modules.Catalog.Dtos
{
    internal record GetCategoryDto(
        Guid Id,
        string Name);
}
