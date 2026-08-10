using System;
using System.Collections.Generic;
using System.Text;

namespace ModularCommerce.Shared.Pagination
{
    public record PaginatedList<T>(IEnumerable<T> Items, int PageNumber, int PageSize, int TotalCount);
    
}
