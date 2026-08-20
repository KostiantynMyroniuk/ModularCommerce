using FluentValidation;

namespace ModularCommerce.Modules.Inventory.Features.Products.CreateInventoryProduct
{
    internal class CreateInventoryProductValidation : AbstractValidator<CreateInventoryProductCommand>
    {
        public CreateInventoryProductValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Description)
                .MaximumLength(1000)
                .When(x => x.Description is not null);

            RuleFor(x => x.Sku)
                .NotEmpty()
                .MaximumLength(64);

            RuleFor(x => x.InitialQuantity)
                .GreaterThanOrEqualTo(0);
        }
    }
}
