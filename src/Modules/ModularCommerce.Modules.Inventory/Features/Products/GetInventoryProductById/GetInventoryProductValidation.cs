using FluentValidation;

namespace ModularCommerce.Modules.Inventory.Features.Products.GetInventoryProductById
{
    internal class GetInventoryProductValidation : AbstractValidator<GetInventoryProductQuery>
    {
        public GetInventoryProductValidation()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty();
        }
    }
}
