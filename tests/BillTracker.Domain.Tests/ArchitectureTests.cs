namespace BillTracker.Domain.Tests;

// AGENTS.md rule 4: the Domain project references nothing but the base class library.
// This is one of the reins: an agent that reaches for a framework inside the domain fails here.
public class ArchitectureTests
{
    private static readonly string[] ForbiddenPrefixes =
    [
        "Microsoft.AspNetCore",
        "Microsoft.EntityFrameworkCore",
        "Microsoft.Extensions",
        "System.Net.Http",
    ];

    [Fact]
    public void Domain_references_only_the_base_class_library()
    {
        var offenders = typeof(DomainAssembly).Assembly.GetReferencedAssemblies()
            .Select(reference => reference.Name ?? string.Empty)
            .Where(name => ForbiddenPrefixes.Any(prefix => name.StartsWith(prefix, StringComparison.Ordinal))
                || name.EndsWith(".Api", StringComparison.Ordinal))
            .ToList();

        Assert.Empty(offenders);
    }
}
