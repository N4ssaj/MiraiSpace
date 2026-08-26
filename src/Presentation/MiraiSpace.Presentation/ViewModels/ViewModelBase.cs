using System.Reactive.Disposables;
using ReactiveUI;

namespace MiraiSpace.Presentation.ViewModels;

/// <summary>
/// Reactive state whose subscriptions follow the activation of its View.
/// </summary>
public abstract class ViewModelBase : ReactiveObject, IActivatableViewModel
{
    protected ViewModelBase()
    {
        this.WhenActivated(OnActivated);
    }

    public ViewModelActivator Activator { get; } = new();

    /// <summary>
    /// Registers work that must exist only while the corresponding View is active.
    /// </summary>
    protected virtual void OnActivated(CompositeDisposable disposables)
    {
    }
}
