using MediatR;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;
using ModularCommerce.Modules.Catalog.Infrastructure;
using ModularCommerce.Modules.Catalog.Models;
using ModularCommerce.Shared.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularCommerce.Modules.Catalog.Features.Products.CreateProduct
{
    internal record CreateProductCommand(
        string Name,
        string? Description,
        string Sku,
        decimal Price,
        Guid CategoryId
        ) : IRequest<Result<Guid>>;

    internal class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<Guid>>
    {
        private readonly CatalogDbContext _context;
        private readonly ILogger<CreateProductCommandHandler> _logger;

        public CreateProductCommandHandler(
            CatalogDbContext context,
            ILogger<CreateProductCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<Guid>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = Product.Create(
                request.Name,
                request.Description,
                request.Sku,
                request.Price,
                request.CategoryId);

            _context.Products.Add(product);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(product.Id);
        }
    }
}
