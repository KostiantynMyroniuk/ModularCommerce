using ModularCommerce.Modules.Catalog.Extensions;
using ModularCommerce.Shared.Enpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddCatalogConfiguration(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "openapi"));
}

app.MapEndpoints();

app.Run();

