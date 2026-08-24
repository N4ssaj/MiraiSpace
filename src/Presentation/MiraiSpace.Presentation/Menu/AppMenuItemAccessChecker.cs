using System.Reactive;
using System.Reactive.Linq;
using MiraiSpace.Extensibility.Abstractions.Menu;

namespace MiraiSpace.Presentation.Menu;

public sealed class AppMenuItemAccessChecker : IAppMenuItemAccessChecker
{
    private readonly IReadOnlyList<IAppMenuItemAccessPolicy> _policies;

    public AppMenuItemAccessChecker(IEnumerable<IAppMenuItemAccessPolicy> policies)
    {
        _policies = policies.ToArray();
        AccessChanged = _policies.Count == 0
            ? Observable.Never<Unit>()
            : _policies.Select(policy => policy.AccessChanged).Merge().Publish().RefCount();
    }

    public IObservable<Unit> AccessChanged { get; }

    public bool CheckAccess(IAppMenuItem item) =>
        _policies.All(policy => policy.CheckAccess(item));
}
