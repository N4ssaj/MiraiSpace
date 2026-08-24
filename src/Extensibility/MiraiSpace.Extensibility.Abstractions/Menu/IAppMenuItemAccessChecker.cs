using System.Reactive;

namespace MiraiSpace.Extensibility.Abstractions.Menu;

public interface IAppMenuItemAccessChecker
{
    bool CheckAccess(IAppMenuItem item);

    IObservable<Unit> AccessChanged { get; }
}
