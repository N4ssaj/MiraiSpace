using System.Reactive;
using MiraiSpace.Presentation.Session;

namespace MiraiSpace.Presentation.Navigation;

public sealed class PageAccess
{
    public string Path { get; }
    public IReadOnlyList<Guid> RequiredRoles { get; }

    public PageAccess(string path, params Guid[] requiredRoles)
    {
        Path = path;
        RequiredRoles = requiredRoles;
    }
}

public sealed class NavigationAccess
{
    private readonly IUserSession _session;
    private readonly PageAccess[] _rules;

    public IObservable<Unit> Invalidated => _session.Changes;

    public NavigationAccess(IUserSession session, IEnumerable<PageAccess> rules)
    {
        _session = session;
        _rules = rules.ToArray();
    }

    public bool IsAllowed(NavigationAddress? address) =>
        _rules.Where(rule => rule.Path == address?.Path)
            .All(rule => rule.RequiredRoles.All(_session.HasRole));

    public void EnsureAllowed(NavigationAddress? address)
    {
        if (!IsAllowed(address))
        {
            throw new InvalidOperationException("Your current roles do not allow this page.");
        }
    }
}
