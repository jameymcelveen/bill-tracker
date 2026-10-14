using System.Reflection;

namespace BillTracker.Api.Endpoints;

public static class EndpointModuleExtensions
{
    // Finds every IEndpointModule in the assembly, in a stable order, and registers its services.
    public static IServiceCollection AddEndpointModules(
        this IServiceCollection services, IConfiguration configuration, Assembly assembly)
    {
        var modules = assembly.GetTypes()
            .Where(type => type is { IsAbstract: false, IsInterface: false }
                && typeof(IEndpointModule).IsAssignableFrom(type))
            .OrderBy(type => type.FullName, StringComparer.Ordinal)
            .Select(type => (IEndpointModule)Activator.CreateInstance(type)!)
            .ToList();

        foreach (var module in modules)
        {
            module.AddServices(services, configuration);
        }

        services.AddSingleton<IReadOnlyList<IEndpointModule>>(modules);
        return services;
    }

    public static WebApplication MapEndpointModules(this WebApplication app)
    {
        foreach (var module in app.Services.GetRequiredService<IReadOnlyList<IEndpointModule>>())
        {
            module.MapEndpoints(app);
        }

        return app;
    }
}
