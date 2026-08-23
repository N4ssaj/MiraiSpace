using System.Reactive;
using MiraiSpace.Extensibility.Abstractions.Menu;

namespace MiraiSpace.Presentation.Menu;

public interface IAppMenuItemAccessChecker
{
    bool CheckAccess(IAppMenuItem item);

    IObservable<Unit> AccessChanged { get; }
}
