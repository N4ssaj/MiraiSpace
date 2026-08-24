using System.Reactive;

namespace MiraiSpace.Extensibility.Abstractions.Menu;

public interface IAppMenuItemAccessPolicy
{
    bool CheckAccess(IAppMenuItem item);

    IObservable<Unit> AccessChanged { get; }
}
