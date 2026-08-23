using System.Reactive;
using MiraiSpace.Extensibility.Abstractions.Menu;

namespace MiraiSpace.Presentation.Menu;

public interface IAppMenuItemAccessPolicy
{
    bool AppliesTo(IAppMenuItem item);

    bool CheckAccess(IAppMenuItem item);

    IObservable<Unit> AccessChanged { get; }
}

public abstract class AppMenuItemAccessPolicy<TCapability> : IAppMenuItemAccessPolicy
    where TCapability : class
{
    public bool AppliesTo(IAppMenuItem item) => item is TCapability;

    public bool CheckAccess(IAppMenuItem item) => CheckAccess((TCapability)item);

    protected abstract bool CheckAccess(TCapability capability);

    public abstract IObservable<Unit> AccessChanged { get; }
}
