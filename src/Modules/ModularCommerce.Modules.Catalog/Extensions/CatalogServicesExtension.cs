using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModularCommerce.Modules.Catalog.Infrastructure;
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
                options.UseSqlServer(configuration.GetConnectionString("CatalogDb"));
            });

            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(CatalogServicesExtension).Assembly);
            });

            services.AddEndpoints(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
