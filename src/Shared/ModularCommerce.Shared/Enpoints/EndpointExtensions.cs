using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ModularCommerce.Shared.Enpoints
{
    public static class EndpointExtensions
    {
        public static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly assembly)
        {
            var descriptors = assembly.DefinedTypes
                .Where(t => t is { IsAbstract: false, IsInterface: false } && t.IsAssignableTo(typeof(IEndpoint)))
                .Select(t => ServiceDescriptor.Transient(typeof(IEndpoint), t.AsType()));

            services.TryAddEnumerable(descriptors);
            return services;
        }

        public static void MapEndpoints(this WebApplication app)
        {
            var endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

            foreach (var endpoint in endpoints)
            {
                endpoint.MapEndpoint(app);
            }
        }
    }
}
