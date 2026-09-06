using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using ReactiveUI;

namespace MiraiSpace.Presentation.Foundation;

public abstract class ReactiveComponent : ReactiveModel, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new();

    protected ReactiveComponent()
    {
        this.WhenActivated(disposables =>
        {
            OnActivated(disposables);
            Disposable.Create(OnDeactivated).DisposeWith(disposables);
        });
    }

    protected virtual void OnActivated(CompositeDisposable disposables)
    {
    }

    protected virtual void OnDeactivated()
    {
    }
}
