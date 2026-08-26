using System.Reactive;

namespace MiraiSpace.Extensibility.Abstractions.Menu;

public interface IAppMenuAccessEvaluator
{
    bool CheckAccess(IAppMenuContribution contribution);

    IObservable<Unit> AccessChanged { get; }
}
