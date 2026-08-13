using ModularCommerce.Modules.Catalog.Extensions;
using ModularCommerce.Modules.Orders.Extensions;
using ModularCommerce.Shared.Enpoints;
using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
});

builder.Services.AddOpenApi();

builder.Services.AddCatalogConfiguration(builder.Configuration);
builder.Services.AddOrdersConfiguration(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await app.UseCatalogMigrations();
    await app.UseOrdersMigrations();

    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "openapi"));
}

app.MapEndpoints();

app.Run();

