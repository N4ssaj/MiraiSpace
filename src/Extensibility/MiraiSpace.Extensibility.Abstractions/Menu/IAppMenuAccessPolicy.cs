using System.Reactive;

namespace MiraiSpace.Extensibility.Abstractions.Menu;

public interface IAppMenuAccessPolicy
{
    bool CheckAccess(IAppMenuContribution contribution);

    IObservable<Unit> AccessChanged { get; }
}
