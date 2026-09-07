namespace MiraiSpace.Presentation.Navigation;

public sealed class NavigationRouter
{
    private readonly NavigationRoute[] _routes;

    public NavigationRouter(IEnumerable<NavigationRoute> routes)
    {
        _routes = routes.ToArray();
        var duplicate = _routes.GroupBy(route => route.Path, StringComparer.OrdinalIgnoreCase)
            .FirstOrDefault(group => group.Count() > 1);
        if (duplicate is not null)
        {
            throw new InvalidOperationException($"Duplicate navigation route: {duplicate.Key}");
        }
    }

    public NavigationRoute Resolve(NavigationAddress address) =>
        _routes.FirstOrDefault(route => route.Matches(address))
        ?? throw new FormatException($"The address '{address.Path}' is not registered.");

    public NavigationAddress? FindAddress(Type pageType)
    {
        var candidates = _routes.Where(item => item.PageType == pageType && !item.Path.Contains('{')).ToArray();
        var route = candidates.Length == 1 ? candidates[0] : null;
        return route is null ? null : NavigationAddress.Parse(route.Path);
    }
}
