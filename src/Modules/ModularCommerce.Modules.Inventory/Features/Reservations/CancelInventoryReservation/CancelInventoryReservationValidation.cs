using FluentValidation;

namespace ModularCommerce.Modules.Inventory.Features.Reservations.CancelInventoryReservation
{
    internal class CancelInventoryReservationValidation : AbstractValidator<CancelInventoryReservationCommand>
    {
        public CancelInventoryReservationValidation()
        {
            RuleFor(x => x.ReservationId)
                .NotEmpty();
        }
    }
}
