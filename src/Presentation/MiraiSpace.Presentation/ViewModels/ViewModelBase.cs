using System.Reactive.Disposables;
using ReactiveUI;

namespace MiraiSpace.Presentation.ViewModels;

public abstract class ViewModelBase : ReactiveObject, IActivatableViewModel, IDisposable
{
    private readonly Lock _initializationGate = new();
    private CancellationTokenSource? _initialization;

    protected ViewModelBase()
    {
        this.WhenActivated(disposables =>
        {
            OnActivated(disposables);
            disposables.Add(Disposable.Create(OnDeactivated));
        });
    }

    public ViewModelActivator Activator { get; } = new();

    protected virtual void OnActivated(CompositeDisposable disposables)
    {
    }

    protected virtual void OnDeactivated()
    {
    }

    protected async ValueTask InitializeLatestAsync(
        Func<CancellationToken, ValueTask> initialize,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(initialize);
        var current = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        CancellationTokenSource? previous;

        lock (_initializationGate)
        {
            previous = _initialization;
            _initialization = current;
        }

        previous?.Cancel();

        try
        {
            await initialize(current.Token);
        }
        finally
        {
            lock (_initializationGate)
            {
                if (ReferenceEquals(_initialization, current))
                {
                    _initialization = null;
                }
            }

            current.Dispose();
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing) return;
        lock (_initializationGate)
        {
            _initialization?.Cancel();
        }
        Activator.Dispose();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
