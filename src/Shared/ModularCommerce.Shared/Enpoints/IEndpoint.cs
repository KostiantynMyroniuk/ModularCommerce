using Microsoft.AspNetCore.Routing;

namespace ModularCommerce.Shared.Enpoints
{
    public interface IEndpoint
    {
        void MapEndpoint(IEndpointRouteBuilder app);
    }
}
