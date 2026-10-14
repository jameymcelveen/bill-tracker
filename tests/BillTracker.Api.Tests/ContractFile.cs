namespace BillTracker.Api.Tests;

// Reads the operations out of contracts/openapi.yaml.
// Deliberately small: paths are indented two spaces under `paths:`, HTTP methods four spaces
// under each path. That is enough to hold the API to the contract without a YAML library.
public static class ContractFile
{
    private static readonly string[] Methods = ["get", "put", "post", "delete", "patch", "head", "options"];

    public static string Locate()
    {
        for (var dir = new DirectoryInfo(AppContext.BaseDirectory); dir is not null; dir = dir.Parent)
        {
            var candidate = Path.Combine(dir.FullName, "contracts", "openapi.yaml");
            if (File.Exists(candidate))
            {
                return candidate;
            }
        }

        throw new FileNotFoundException($"contracts/openapi.yaml was not found above {AppContext.BaseDirectory}");
    }

    public static IReadOnlySet<string> Operations(string path)
    {
        var operations = new HashSet<string>(StringComparer.Ordinal);
        var inPaths = false;
        string? currentPath = null;

        foreach (var raw in File.ReadLines(path))
        {
            var line = raw.TrimEnd();
            if (line.Length == 0 || line.TrimStart().StartsWith('#'))
            {
                continue;
            }

            if (!char.IsWhiteSpace(line[0]))
            {
                inPaths = line.StartsWith("paths:", StringComparison.Ordinal);
                currentPath = null;
                continue;
            }

            if (!inPaths)
            {
                continue;
            }

            if (IndentOf(line) == 2 && line.TrimStart().StartsWith('/'))
            {
                currentPath = line.Trim().TrimEnd(':');
            }
            else if (IndentOf(line) == 4 && currentPath is not null)
            {
                var key = line.Trim().TrimEnd(':');
                if (Methods.Contains(key))
                {
                    operations.Add($"{key.ToUpperInvariant()} {currentPath}");
                }
            }
        }

        return operations;
    }

    private static int IndentOf(string line) => line.Length - line.TrimStart(' ').Length;
}
