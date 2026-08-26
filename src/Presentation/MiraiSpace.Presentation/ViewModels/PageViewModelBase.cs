using System.Reactive.Disposables;

namespace MiraiSpace.Presentation.ViewModels;

/// <summary>
/// Base for route-addressable content hosted by the application shell.
/// </summary>
public abstract class PageViewModelBase(string route, string title) : ViewModelBase
{
    public string Route { get; } =
        !string.IsNullOrWhiteSpace(route)
            ? route
            : throw new ArgumentException("A page route is required.", nameof(route));

    public string Title { get; } =
        !string.IsNullOrWhiteSpace(title)
            ? title
            : throw new ArgumentException("A page title is required.", nameof(title));

    protected sealed override void OnActivated(CompositeDisposable disposables)
    {
        var cancellation = new CancellationTokenSource();
        disposables.Add(Disposable.Create(() =>
        {
            cancellation.Cancel();
            cancellation.Dispose();
        }));
        OnPageActivated(disposables, cancellation.Token);
    }

    protected virtual void OnPageActivated(
        CompositeDisposable disposables,
        CancellationToken cancellationToken)
    {
    }
}
