using ModularCommerce.API.Behaviors;
using ModularCommerce.API.Middlewares;
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

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddCatalogConfiguration(builder.Configuration);
builder.Services.AddOrdersConfiguration(builder.Configuration);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);

    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await app.UseCatalogMigrations();
    await app.UseOrdersMigrations();

    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "openapi"));
}

app.UseExceptionHandler();

app.MapEndpoints();

app.Run();

