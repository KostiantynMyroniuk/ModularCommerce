using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModularCommerce.Modules.Orders.Infrastructure;
using ModularCommerce.Shared.Enpoints;

namespace ModularCommerce.Modules.Orders.Extensions
{
    public static class OrdersServicesExtension
    {
        public static IServiceCollection AddOrdersConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<OrdersDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("ModularCommerceDb"));
            });

            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(OrdersServicesExtension).Assembly);
            });

            services.AddEndpoints(typeof(OrdersServicesExtension).Assembly);

            return services;
        }

        public static async Task UseOrdersMigrations(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<OrdersDbContext>();
            await context.Database.MigrateAsync();
        }
    }
}
