using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModularCommerce.Modules.Catalog.Abstractions;
using ModularCommerce.Modules.Catalog.Infrastructure;
using ModularCommerce.Modules.Catalog.Infrastructure.Services;
using ModularCommerce.Shared.Enpoints;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ModularCommerce.Modules.Catalog.Extensions
{
    public static class CatalogServicesExtension
    {
        public static IServiceCollection AddCatalogConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CatalogDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("ModularCommerceDb"));
            });

            services.AddScoped<IProductCatalogReader, ProductCatalogReader>();

            services.AddValidatorsFromAssembly(typeof(CatalogServicesExtension).Assembly, includeInternalTypes: true);

            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(CatalogServicesExtension).Assembly);
            });

            services.AddEndpoints(Assembly.GetExecutingAssembly());

            return services;
        }

        public static async Task UseCatalogMigrations(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
            await context.Database.MigrateAsync();
        }
    }
}
