using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModularCommerce.Modules.Inventory.Infrastructure;
using ModularCommerce.Shared.Enpoints;
using System.Reflection;

namespace ModularCommerce.Modules.Inventory.Extensions
{
    public static class InventoryServicesExtension
    {
        public static IServiceCollection AddInventoryConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<InventoryDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("ModularCommerceDb"));
            });

            services.AddValidatorsFromAssembly(typeof(InventoryServicesExtension).Assembly, includeInternalTypes: true);

            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(InventoryServicesExtension).Assembly);
            });

            services.AddEndpoints(Assembly.GetExecutingAssembly());

            return services;
        }

        public static async Task UseInventoryMigrations(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();
            await context.Database.MigrateAsync();
        }
    }
}
