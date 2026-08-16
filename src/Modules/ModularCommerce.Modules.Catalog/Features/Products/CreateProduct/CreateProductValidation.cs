using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularCommerce.Modules.Catalog.Features.Products.CreateProduct
{
    internal class CreateProductValidation : AbstractValidator<CreateProductCommand>
    {
        public CreateProductValidation()
        {
            RuleFor(p => p.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(p => p.Price)
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(p => p.Description)
                .MaximumLength(512);

            RuleFor(p => p.Sku)
                .NotEmpty()
                .Length(8, 16);
        }
    }
}
