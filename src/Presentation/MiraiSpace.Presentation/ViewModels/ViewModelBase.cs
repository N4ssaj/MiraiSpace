using System.Reactive.Disposables;
using ReactiveUI;

namespace MiraiSpace.Presentation.ViewModels;

/// <summary>
/// Owns the reactive lifetime of a presentation model.
/// </summary>
public abstract class ViewModelBase : ReactiveObject, IDisposable
{
    private readonly CompositeDisposable _lifetime = new();

    protected T Own<T>(T disposable)
        where T : IDisposable
    {
        ArgumentNullException.ThrowIfNull(disposable);
        _lifetime.Add(disposable);
        return disposable;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _lifetime.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
