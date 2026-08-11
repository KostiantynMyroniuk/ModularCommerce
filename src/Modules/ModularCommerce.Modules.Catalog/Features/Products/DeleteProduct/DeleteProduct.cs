using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ModularCommerce.Modules.Catalog.Infrastructure;
using ModularCommerce.Shared.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularCommerce.Modules.Catalog.Features.Products.DeleteProduct
{
    internal record DeleteProductCommand(Guid ProductId) : IRequest<Result>;

    internal class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Result>
    {
        private readonly CatalogDbContext _context;
        private readonly ILogger<DeleteProductCommandHandler> _logger;

        public DeleteProductCommandHandler(
            CatalogDbContext context,
            ILogger<DeleteProductCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var deletedProducts = await _context.Products
                .Where(p => p.Id == request.ProductId)
                .ExecuteDeleteAsync(cancellationToken);

            if (deletedProducts == 0)
            {
                _logger.LogWarning("Product {ProductId} not found", request.ProductId);
                return Result.Fail($"Product {request.ProductId} not found");
            }

            _logger.LogInformation("Product {ProductId} was deleted", request.ProductId);
            return Result.Success();
        }
    }
}
