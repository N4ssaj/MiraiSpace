using DialogHostAvalonia;

namespace MiraiSpace.UI.Dialogs;

internal sealed class DialogRegion : IDisposable
{
    private readonly CancellationTokenSource _lifetime = new();
    private bool _disposed;

    public DialogHost Host { get; }
    public bool IsOpen { get; set; }
    public CancellationToken Lifetime => _lifetime.Token;

    public DialogRegion(DialogHost host)
    {
        Host = host;
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        _lifetime.Cancel();
        _lifetime.Dispose();
    }
}
