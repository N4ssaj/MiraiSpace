using System.Reactive;

namespace MiraiSpace.Extensibility.Abstractions.Menu;

public interface IAppMenuAccessPolicy
{
    IObservable<Unit> Invalidated { get; }

    bool CanAccess(IAppMenuItem item);
}
