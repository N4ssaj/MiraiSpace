using System.Reactive.Disposables;
using MiraiSpace.Presentation.Abstractions.Lifecycle;
using ReactiveUI;

namespace MiraiSpace.Presentation.ViewModels;

public abstract class ViewModelBase : ReactiveObject, IActivatableViewModel, IInitializable, IDisposable
{
    private readonly Lock _initializationGate = new();
    private CancellationTokenSource? _initialization;
    private bool _isDisposed;

    protected ViewModelBase()
    {
        this.WhenActivated(disposables =>
        {
            OnActivated(disposables);
            disposables.Add(Disposable.Create(OnDeactivated));
        });
    }

    public ViewModelActivator Activator { get; } = new();

    public ValueTask InitializeAsync(CancellationToken cancellationToken = default) =>
        InitializeLatestAsync(OnInitializeAsync, cancellationToken);

    protected virtual ValueTask OnInitializeAsync(CancellationToken cancellationToken) =>
        ValueTask.CompletedTask;

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
        ObjectDisposedException.ThrowIf(_isDisposed, this);
        var current = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        CancellationTokenSource? previous;

        lock (_initializationGate)
        {
            previous = _initialization;
            _initialization = current;
        }

        try
        {
            previous?.Cancel();
        }
        catch (ObjectDisposedException)
        {
            // The replaced initialization completed concurrently.
        }

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
        if (_isDisposed) return;
        _isDisposed = true;
        lock (_initializationGate)
        {
            try
            {
                _initialization?.Cancel();
            }
            catch (ObjectDisposedException)
            {
                // The initialization completed concurrently with final disposal.
            }
            _initialization = null;
        }
        Activator.Dispose();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
