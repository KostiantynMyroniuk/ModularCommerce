using FluentValidation;

namespace ModularCommerce.Modules.Inventory.Features.Products.AdjustInventoryQuantity
{
    internal class AdjustInventoryQuantityValidation : AbstractValidator<AdjustInventoryQuantityCommand>
    {
        public AdjustInventoryQuantityValidation()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty();

            RuleFor(x => x.QuantityDelta)
                .NotEqual(0);
        }
    }
}
