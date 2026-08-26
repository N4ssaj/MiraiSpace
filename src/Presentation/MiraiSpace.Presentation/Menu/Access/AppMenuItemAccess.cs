using System.Reactive;
using System.Reactive.Linq;
using MiraiSpace.Presentation.Abstractions.Menu;

namespace MiraiSpace.Presentation.Menu.Access;

public interface IAppMenuItemAccessPolicy
{
    bool CheckAccess(IAppMenuItem item);

    IObservable<Unit> AccessChanged { get; }
}

public interface IAppMenuItemAccess
{
    bool CheckAccess(IAppMenuItem item);

    IObservable<Unit> AccessChanged { get; }
}

public sealed class AppMenuItemAccess(IEnumerable<IAppMenuItemAccessPolicy> policies)
    : IAppMenuItemAccess
{
    private readonly IReadOnlyList<IAppMenuItemAccessPolicy> _policies = policies.ToArray();

    public IObservable<Unit> AccessChanged => _policies.Count == 0
        ? Observable.Never<Unit>()
        : _policies.Select(policy => policy.AccessChanged).Merge();

    public bool CheckAccess(IAppMenuItem item) =>
        _policies.All(policy => policy.CheckAccess(item));
}
