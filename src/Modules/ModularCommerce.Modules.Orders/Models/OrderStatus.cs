namespace ModularCommerce.Modules.Orders.Models
{
    internal enum OrderStatus
    {
        Pending = 0,
        Confirmed = 1,
        Processing = 2,
        Shipped = 3,
        Completed = 4,
        Cancelled = 5
    }
}
