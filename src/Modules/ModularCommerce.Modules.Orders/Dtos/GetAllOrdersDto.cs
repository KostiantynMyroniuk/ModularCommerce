using ModularCommerce.Modules.Orders.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularCommerce.Modules.Orders.Dtos
{
    internal record GetAllOrdersDto(
        Guid OrderId, 
        string OrderNumber,
        OrderStatus Status,
        decimal TotalAmount);

}
