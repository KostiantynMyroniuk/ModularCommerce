using FluentValidation;

namespace ModularCommerce.Modules.Inventory.Features.Reservations.ReserveInventoryStock
{
    internal class ReserveInventoryStockValidation : AbstractValidator<ReserveInventoryStockCommand>
    {
        public ReserveInventoryStockValidation()
        {
            RuleFor(x => x.RequestId)
                .NotEmpty();

            RuleFor(x => x.ProductId)
                .NotEmpty();

            RuleFor(x => x.Quantity)
                .GreaterThan(0);
        }
    }
}
