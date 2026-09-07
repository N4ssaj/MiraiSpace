using System.Collections.ObjectModel;

namespace MiraiSpace.Presentation.Navigation;

/// <summary>An application-relative URL. It never opens a browser or accesses the filesystem.</summary>
public sealed record NavigationAddress
{
    public string Path { get; }
    public string Fragment { get; }
    public IReadOnlyDictionary<string, string> Query { get; }
    public string Url { get; }

    private NavigationAddress(string path, string fragment, Dictionary<string, string> query, string url)
    {
        Path = path;
        Fragment = fragment;
        Query = new ReadOnlyDictionary<string, string>(query);
        Url = url;
    }

    public static NavigationAddress Parse(string url)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(url);
        url = url.Trim();
        if (!url.StartsWith('/') || url.StartsWith("//") || url.Contains('\\') || url.Any(char.IsControl))
            throw new FormatException("Use an application URL beginning with a single '/'.");

        var fragmentIndex = url.IndexOf('#');
        var fragment = fragmentIndex < 0 ? "" : Uri.UnescapeDataString(url[(fragmentIndex + 1)..]);
        var withoutFragment = fragmentIndex < 0 ? url : url[..fragmentIndex];
        var queryIndex = withoutFragment.IndexOf('?');
        var path = (queryIndex < 0 ? withoutFragment : withoutFragment[..queryIndex]).TrimEnd('/');
        if (path.Length == 0) path = "/";
        if (path != "/" && path.Split('/').Skip(1).Any(s => s.Length == 0 || Uri.UnescapeDataString(s) is "." or ".."))
            throw new FormatException("Empty and relative path segments are not supported.");

        var query = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        if (queryIndex >= 0)
        {
            foreach (var pair in withoutFragment[(queryIndex + 1)..].Split('&', StringSplitOptions.RemoveEmptyEntries))
            {
                var parts = pair.Split('=', 2);
                var key = Uri.UnescapeDataString(parts[0].Replace('+', ' '));
                if (!query.TryAdd(key, parts.Length == 2 ? Uri.UnescapeDataString(parts[1].Replace('+', ' ')) : ""))
                    throw new FormatException($"Duplicate query parameter '{key}'.");
            }
        }
        var normalized = path + (queryIndex < 0 ? "" : withoutFragment[queryIndex..]) +
                         (fragmentIndex < 0 ? "" : url[fragmentIndex..]);
        return new NavigationAddress(path, fragment, query, normalized);
    }

    public override string ToString() => Url;
}
