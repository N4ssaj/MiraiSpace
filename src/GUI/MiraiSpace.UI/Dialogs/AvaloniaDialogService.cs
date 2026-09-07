using Avalonia.Controls;
using System.ComponentModel;
using System.Reactive.Disposables;
using Avalonia.Threading;
using DialogHostAvalonia;
using HanumanInstitute.MvvmDialogs;
using MiraiSpace.Presentation.Lifecycle;
using ReactiveUI;
using IDialogService = MiraiSpace.Presentation.Dialogs.IDialogService;
using ViewLocator = MiraiSpace.UI.Infrastructure.ViewLocator;

namespace MiraiSpace.UI.Dialogs;

public sealed class AvaloniaDialogService : IDialogService, IDisposable
{
    private readonly ViewLocator _views;
    private readonly Dictionary<INotifyPropertyChanged, DialogRegion> _regions = new(ReferenceEqualityComparer.Instance);
    private bool _disposed;

    public AvaloniaDialogService(ViewLocator views)
    {
        _views = views;
    }

    internal IDisposable Register(INotifyPropertyChanged owner, DialogHost host)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        var region = new DialogRegion(host);
        if (!_regions.TryAdd(owner, region))
        {
            region.Dispose();
            throw new InvalidOperationException("This model already owns a dialog region.");
        }

        return Disposable.Create(() =>
        {
            _regions.Remove(owner);
            region.Dispose();
        });
    }

    public Task<bool?> ShowAsync<TDialog>(
        INotifyPropertyChanged owner,
        TDialog dialog,
        CancellationToken cancellationToken = default)
        where TDialog : class, IModalDialogViewModel, ICloseable =>
        Dispatcher.UIThread.InvokeAsync(() => ShowCoreAsync(
            owner,
            dialog,
            token => dialog is IInitializable initializable
                ? initializable.InitializeAsync(token)
                : ValueTask.CompletedTask,
            cancellationToken));

    public Task<bool?> ShowAsync<TDialog, TParameters>(
        INotifyPropertyChanged owner,
        TDialog dialog,
        TParameters parameters,
        CancellationToken cancellationToken = default)
        where TDialog : class, IModalDialogViewModel, ICloseable, IInitializable<TParameters> =>
        Dispatcher.UIThread.InvokeAsync(() => ShowCoreAsync(
            owner,
            dialog,
            token => dialog.InitializeAsync(parameters, token),
            cancellationToken));

    private async Task<bool?> ShowCoreAsync<TDialog>(
        INotifyPropertyChanged owner,
        TDialog dialog,
        Func<CancellationToken, ValueTask> initialize,
        CancellationToken cancellationToken)
        where TDialog : class, IModalDialogViewModel, ICloseable
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        if (!_regions.TryGetValue(owner, out var region))
        {
            throw new InvalidOperationException("The owner has no visible dialog region.");
        }

        using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, region.Lifetime);
        if (region.IsOpen)
        {
            throw new InvalidOperationException("The region already has an open dialog.");
        }

        region.IsOpen = true;
        try
        {
            await initialize(linked.Token);
            linked.Token.ThrowIfCancellationRequested();
            var view = _views.Build(dialog)
                ?? throw new InvalidOperationException($"No view is registered for {dialog.GetType().Name}.");
            view.DataTemplates.Add(_views);
            ((IViewFor)view).ViewModel = dialog;
            return await ShowViewAsync(region, dialog, view, linked.Token);
        }
        finally
        {
            region.IsOpen = false;
        }
    }

    private static async Task<bool?> ShowViewAsync<TDialog>(
        DialogRegion region,
        TDialog dialog,
        Control view,
        CancellationToken token)
        where TDialog : IModalDialogViewModel, ICloseable
    {
        DialogSession? session = null;
        void Close(object? sender, EventArgs args) =>
            Dispatcher.UIThread.Post(() =>
            {
                if (session is { IsEnded: false })
                {
                    session.Close(dialog.DialogResult);
                }
            });

        dialog.RequestClose += Close;
        using var cancellation = token.Register(() => Dispatcher.UIThread.Post(() =>
        {
            if (session is { IsEnded: false })
            {
                session.Close(null);
            }
        }));
        try
        {
            var result = await DialogHost.Show(
                view,
                region.Host,
                openedEventHandler: (_, args) =>
                {
                    session = args.Session;
                    if (token.IsCancellationRequested)
                    {
                        session.Close(null);
                    }
                });
            token.ThrowIfCancellationRequested();
            return result as bool?;
        }
        finally
        {
            dialog.RequestClose -= Close;
        }
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        foreach (var region in _regions.Values)
        {
            region.Dispose();
        }

        _regions.Clear();
    }
}
