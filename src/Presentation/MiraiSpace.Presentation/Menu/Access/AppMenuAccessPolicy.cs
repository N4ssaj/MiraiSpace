using System.Reactive;
using System.Reactive.Linq;
using MiraiSpace.Extensibility.Abstractions.Menu;

namespace MiraiSpace.Presentation.Menu.Access;

public abstract class AppMenuAccessPolicy<TCapability> : IAppMenuAccessPolicy
    where TCapability : class
{
    public virtual IObservable<Unit> Invalidated => Observable.Never<Unit>();

    public bool CanAccess(IAppMenuItem item)
    {
        ArgumentNullException.ThrowIfNull(item);
        return item is not TCapability capability || CanAccess(capability);
    }

    protected abstract bool CanAccess(TCapability capability);
}
