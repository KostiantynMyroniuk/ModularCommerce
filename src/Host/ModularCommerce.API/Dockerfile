FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["src/Host/ModularCommerce.API/ModularCommerce.API.csproj", "src/Host/ModularCommerce.API/"]
COPY ["src/Modules/ModularCommerce.Modules.Orders/ModularCommerce.Modules.Orders.csproj", "src/Modules/ModularCommerce.Modules.Orders/"]
COPY ["src/Modules/ModularCommerce.Modules.Catalog/ModularCommerce.Modules.Catalog.csproj", "src/Modules/ModularCommerce.Modules.Catalog/"]
COPY ["src/Modules/ModularCommerce.Modules.Inventory/ModularCommerce.Modules.Inventory.csproj", "src/Modules/ModularCommerce.Modules.Inventory/"]
COPY ["src/Shared/ModularCommerce.Shared/ModularCommerce.Shared.csproj", "src/Shared/ModularCommerce.Shared/"]
RUN dotnet restore "./src/Host/ModularCommerce.API/ModularCommerce.API.csproj"
COPY . .
WORKDIR "/src/src/Host/ModularCommerce.API"
RUN dotnet build "./ModularCommerce.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ModularCommerce.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ModularCommerce.API.dll"]