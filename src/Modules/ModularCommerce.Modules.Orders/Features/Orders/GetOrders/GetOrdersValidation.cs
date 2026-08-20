using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularCommerce.Modules.Orders.Features.Orders.GetOrders
{
    internal class GetOrdersValidation : AbstractValidator<GetOrdersQuery>
    {
        public GetOrdersValidation()
        {
            RuleFor(o => o.PageNumber)
                .NotNull()
                .GreaterThan(1);

            RuleFor(o => o.PageSize)
                .NotNull()
                .InclusiveBetween(1, 25);
        }
    }
}
