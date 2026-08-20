using MediatR;
using Microsoft.Build.Framework;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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

            try
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Product {ProductId} created successfully", product.Id);
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException { Number: 2627 or 2601})
            {
                _logger.LogWarning("Duplicate SKU detected: {Sku} while creating product", product.Sku);

                _context.ChangeTracker.Clear();

                return Result<Guid>.Fail($"Product with SKU: {product.Sku} already exists");
            }
            
            return Result<Guid>.Success(product.Id);
        }
    }
}
