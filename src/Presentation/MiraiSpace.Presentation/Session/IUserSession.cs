using System.Reactive;

namespace MiraiSpace.Presentation.Session;

public interface IUserSession
{
    bool HasRole(Guid roleId);
    IObservable<Unit> Changes { get; }
}
