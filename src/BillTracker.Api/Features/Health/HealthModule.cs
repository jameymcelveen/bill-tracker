using BillTracker.Api.Endpoints;

namespace BillTracker.Api.Features.Health;

public sealed class HealthModule : IEndpointModule
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/health", () => Results.Ok(new { status = "ok" }));
    }
}
