using MediatR;

namespace ModularCommerce.Shared.Contracts.Orders
{
    public sealed record OrderCreatedItemNotification(
        Guid ProductId,
        int Quantity);

    public sealed record OrderCreatedNotification(
        Guid RequestId,
        IReadOnlyCollection<OrderCreatedItemNotification> Items) : INotification;
}
