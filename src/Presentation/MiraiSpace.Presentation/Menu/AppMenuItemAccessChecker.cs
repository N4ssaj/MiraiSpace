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
            : _policies.Select(x => x.AccessChanged).Merge().Publish().RefCount();
    }

    public IObservable<Unit> AccessChanged { get; }

    public bool CheckAccess(IAppMenuItem item) =>
        _policies.Where(x => x.AppliesTo(item)).All(x => x.CheckAccess(item));
}
