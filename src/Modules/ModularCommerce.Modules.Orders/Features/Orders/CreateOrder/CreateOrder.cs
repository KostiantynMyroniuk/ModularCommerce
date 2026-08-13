using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ModularCommerce.Modules.Catalog.Abstractions;
using ModularCommerce.Modules.Orders.Infrastructure;
using ModularCommerce.Modules.Orders.Models;
using ModularCommerce.Shared.Results;

namespace ModularCommerce.Modules.Orders.Features.Orders.CreateOrder
{
    internal record AddressDto(string Address1, string? Address2, string City, string Country, string? Region, string? PostalCode);

    internal record OrderItemDto(
        Guid ProductId,
        int Quantity);

    internal record CreateOrderCommand(
        Guid RequestId,
        AddressDto Address,
        IReadOnlyCollection<OrderItemDto> Items) : IRequest<Result<Guid>>;

    internal class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Result<Guid>>
    {
        private readonly OrdersDbContext _context;
        private readonly IProductCatalogReader _productCatalogReader;
        private readonly IMediator _mediator;
        private readonly ILogger<CreateOrderCommandHandler> _logger;

        public CreateOrderCommandHandler(
            OrdersDbContext context,
            IProductCatalogReader productCatalogReader,
            IMediator mediator,
            ILogger<CreateOrderCommandHandler> logger)
        {
            _context = context;
            _productCatalogReader = productCatalogReader;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<Result<Guid>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var productIds = request.Items.Select(item => item.ProductId).ToArray();
            var productsResult = await _productCatalogReader.GetProductsAsync(productIds, cancellationToken);

            if (productsResult.IsFailure || productsResult.Value is null)
            {
                return Result<Guid>.Fail(productsResult.ErrorMessage ?? "Failed to load products.");
            }

            var productsById = productsResult.Value.ToDictionary(product => product.ProductId);

            var order = Order.Create(
                request.RequestId,
                request.Address.Address1,
                request.Address.Address2,
                request.Address.City,
                request.Address.Country,
                request.Address.Region,
                request.Address.PostalCode,
                request.Items.Select(item => OrderItem.Create(
                    productsById[item.ProductId].ProductId,
                    productsById[item.ProductId].Name,
                    productsById[item.ProductId].Description,
                    productsById[item.ProductId].Sku,
                    productsById[item.ProductId].Price,
                    item.Quantity)));

            try
            {
                _context.Orders.Add(order);
                await _context.SaveChangesAsync(cancellationToken);

                return Result<Guid>.Success(order.Id);
            }
            catch (DbUpdateException exception) when (IsUniqueConstraintViolation(exception))
            {
                _context.ChangeTracker.Clear();

                var existingOrder = await _context.Orders
                    .AsNoTracking()
                    .FirstOrDefaultAsync(currentOrder => currentOrder.RequestId == request.RequestId, cancellationToken);

                if (existingOrder is not null)
                {
                    _logger.LogInformation(
                        "Order {OrderId} already exists for RequestId {RequestId}",
                        existingOrder.Id,
                        request.RequestId);

                    return Result<Guid>.Success(existingOrder.Id);
                }

                _logger.LogWarning(exception, "Unique constraint violation while creating order for RequestId {RequestId}", request.RequestId);
                return Result<Guid>.Fail("Order already exists.");
            }
        }

        private static bool IsUniqueConstraintViolation(DbUpdateException exception)
        {
            return exception.InnerException is Microsoft.Data.SqlClient.SqlException sqlException
                && (sqlException.Number == 2601 || sqlException.Number == 2627);
        }
    }
}
