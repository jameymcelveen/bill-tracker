namespace BillTracker.Api.Endpoints;

// One feature's slice of the HTTP surface: its services and its routes, in one place.
// Every path a module maps must already be in contracts/openapi.yaml (the contract test checks).
public interface IEndpointModule
{
    void AddServices(IServiceCollection services, IConfiguration configuration)
    {
    }

    void MapEndpoints(IEndpointRouteBuilder app);
}
