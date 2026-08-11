using MediatR;
using Microsoft.EntityFrameworkCore;
using ModularCommerce.Modules.Catalog.Dtos;
using ModularCommerce.Modules.Catalog.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularCommerce.Modules.Catalog.Features.Categories.GetAllCategories
{
    internal record GetAllCategoriesQuery(Guid? ParentId) : IRequest<List<GetCategoryDto>>;

    internal class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, List<GetCategoryDto>>
    {
        private readonly CatalogDbContext _context; 

        public GetAllCategoriesQueryHandler(CatalogDbContext context)
        {
            _context = context;
        }

        public async Task<List<GetCategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _context.Categories
                .AsNoTracking()
                .Where(c => c.ParentCategoryId == request.ParentId)
                .Select(c => new GetCategoryDto(
                    c.Id,
                    c.CategoryName))
                .ToListAsync(cancellationToken);

            return categories;
        }
    }
}
