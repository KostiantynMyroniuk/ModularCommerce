using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularCommerce.Modules.Orders.Features.Orders.CreateOrder
{
    internal class CreateOrderValidation : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderValidation()
        {
            RuleFor(o => o.Address.Address1)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(o => o.Address.Address2)
                .MaximumLength(200);

            RuleFor(o => o.Address.City)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(o => o.Address.Country)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(o => o.Address.Region)
                .MaximumLength(100);

            RuleFor(o => o.Address.PostalCode)
                .MaximumLength(20);

            RuleFor(o => o.Items)
                .NotEmpty();

            RuleForEach(o => o.Items)
                .SetValidator(new CreateOrderItemValidation());
        }
    }

    internal class CreateOrderItemValidation : AbstractValidator<OrderItemDto>
    {
        public CreateOrderItemValidation()
        {
            RuleFor(i => i.ProductId)
                .NotEmpty();

            RuleFor(i => i.Quantity)
                .GreaterThan(0);
        }
    }
}
