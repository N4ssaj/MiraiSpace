using System.Reactive.Disposables;

namespace MiraiSpace.Presentation.ViewModels;

/// <summary>
/// Base for an independently rendered, activatable part of a View.
/// </summary>
public abstract class ComponentViewModelBase(string componentId) : ViewModelBase
{
    public string ComponentId { get; } =
        !string.IsNullOrWhiteSpace(componentId)
            ? componentId
            : throw new ArgumentException("A component identifier is required.", nameof(componentId));

    protected sealed override void OnActivated(CompositeDisposable disposables) =>
        OnComponentActivated(disposables);

    protected virtual void OnComponentActivated(CompositeDisposable disposables)
    {
    }
}
