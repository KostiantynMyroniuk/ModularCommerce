using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularCommerce.Modules.Catalog.Features.Products.GetProductsPaginated
{
    internal class GetProductsPaginatedValidation : AbstractValidator<GetProductsPaginatedQuery>
    {
        public GetProductsPaginatedValidation()
        {
            RuleFor(p => p.PageNumber)
                .NotNull()
                .GreaterThan(0);

            RuleFor(p => p.PageSize)
                .NotNull()
                .InclusiveBetween(1, 25);
        }
    }
}
