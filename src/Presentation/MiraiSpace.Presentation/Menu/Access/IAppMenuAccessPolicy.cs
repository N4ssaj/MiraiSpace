using System.Reactive;
using MiraiSpace.Presentation.Abstractions.Menu;

namespace MiraiSpace.Presentation.Menu.Access;

public interface IAppMenuAccessPolicy
{
    bool CheckAccess(IAppMenuItem item);
    IObservable<Unit> AccessChanged { get; }
}
