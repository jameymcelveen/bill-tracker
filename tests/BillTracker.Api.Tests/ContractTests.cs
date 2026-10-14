using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace BillTracker.Api.Tests;

// AGENTS.md rule 3: the contract is the law. Every path and method in contracts/openapi.yaml
// is served, and nothing is served that the contract does not list.
public class ContractTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public ContractTests(WebApplicationFactory<Program> factory)
    {
        // Development startup applies EF migrations. This test only checks routes,
        // so it hosts the app outside Development and never opens a database.
        _factory = factory.WithWebHostBuilder(builder => builder.UseEnvironment("Testing"));
    }

    [Fact]
    public void The_api_serves_exactly_the_contract()
    {
        var contract = ContractFile.Operations(ContractFile.Locate());

        var served = _factory.Services.GetRequiredService<EndpointDataSource>().Endpoints
            .OfType<RouteEndpoint>()
            .SelectMany(endpoint =>
                (endpoint.Metadata.GetMetadata<HttpMethodMetadata>()?.HttpMethods ?? [])
                .Select(method => $"{method.ToUpperInvariant()} {endpoint.RoutePattern.RawText}"))
            .ToHashSet(StringComparer.Ordinal);

        Assert.Equal(
            contract.OrderBy(operation => operation, StringComparer.Ordinal),
            served.OrderBy(operation => operation, StringComparer.Ordinal));
    }
}
