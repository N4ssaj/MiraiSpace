using System.Reactive;
using System.Reactive.Linq;
using MiraiSpace.Extensibility.Abstractions.Menu;

namespace MiraiSpace.Presentation.Menu;

public sealed class AppMenuAccessEvaluator : IAppMenuAccessEvaluator
{
    private readonly IReadOnlyList<IAppMenuAccessPolicy> _policies;

    public AppMenuAccessEvaluator(IEnumerable<IAppMenuAccessPolicy> policies)
    {
        _policies = policies.ToArray();
        AccessChanged = _policies.Count == 0
            ? Observable.Never<Unit>()
            : _policies.Select(policy => policy.AccessChanged).Merge().Publish().RefCount();
    }

    public IObservable<Unit> AccessChanged { get; }

    public bool CheckAccess(IAppMenuContribution contribution) =>
        _policies.All(policy => policy.CheckAccess(contribution));
}
