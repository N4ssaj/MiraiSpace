using System.Collections.ObjectModel;
using Microsoft.Extensions.DependencyInjection;
using MiraiSpace.Presentation.Foundation;
using MiraiSpace.Presentation.Lifecycle;
using MiraiSpace.Presentation.Navigation;

namespace MiraiSpace.Presentation.Panels;

public sealed class PanelService : IPanelService, IAsyncDisposable, IDisposable
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IUiDispatcher _dispatcher;
    private readonly ObservableCollection<NavigationPanelViewModel> _panels = [];
    private readonly ReadOnlyObservableCollection<NavigationPanelViewModel> _readOnlyPanels;
    private readonly Dictionary<Guid, AsyncServiceScope> _scopes = [];

    private bool _disposed;

    public IReadOnlyList<NavigationPanelViewModel> Panels => _readOnlyPanels;

    public PanelService(IServiceScopeFactory scopeFactory, IUiDispatcher dispatcher)
    {
        _scopeFactory = scopeFactory;
        _dispatcher = dispatcher;
        _readOnlyPanels = new ReadOnlyObservableCollection<NavigationPanelViewModel>(_panels);
    }

    public Task<Guid> OpenAsync(string address, CancellationToken cancellationToken = default) =>
        OpenCoreAsync(navigation => navigation.NavigateAsync(address, cancellationToken));

    public Task<Guid> OpenAsync<TPage, TParameters>(
        TParameters parameters,
        CancellationToken cancellationToken = default)
        where TPage : ReactivePage, IInitializable<TParameters> =>
        OpenCoreAsync(navigation => navigation.NavigateAsync<TPage, TParameters>(parameters, cancellationToken));

    private Task<Guid> OpenCoreAsync(Func<INavigationService, Task> navigate) =>
        _dispatcher.InvokeAsync(async () =>
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            var scope = _scopeFactory.CreateAsyncScope();
            NavigationPanelViewModel? panel = null;
            try
            {
                panel = scope.ServiceProvider.GetRequiredService<NavigationPanelViewModel>();
                _scopes.Add(panel.Id, scope);
                _panels.Add(panel);
                await navigate(panel.Navigation);
                return panel.Id;
            }
            catch
            {
                if (panel is not null)
                {
                    await CloseAsync(panel.Id);
                }
                else
                {
                    await scope.DisposeAsync();
                }

                throw;
            }
        });

    public async Task CloseAsync(Guid id)
    {
        await _dispatcher.InvokeAsync(async () =>
        {
            if (_scopes.Remove(id, out var scope))
            {
                var navigation = scope.ServiceProvider.GetRequiredService<INavigationService>();
                if (navigation is IAsyncDisposable lifetime)
                {
                    await lifetime.DisposeAsync();
                }

                await scope.DisposeAsync();
                var panel = _panels.First(item => item.Id == id);
                _panels.Remove(panel);
            }

            return true;
        });
    }

    public Task NavigateAsync(Guid id, string address, CancellationToken cancellationToken = default) =>
        _dispatcher.InvokeAsync(async () =>
        {
            var panel = _panels.First(item => item.Id == id);
            await panel.Navigation.NavigateAsync(address, cancellationToken);
            return true;
        });

    public Task GoBackAsync(Guid id, CancellationToken cancellationToken = default) =>
        _dispatcher.InvokeAsync(async () =>
        {
            var panel = _panels.First(item => item.Id == id);
            await panel.Navigation.GoBackAsync(cancellationToken);
            return true;
        });

    public void Dispose()
    {
        _disposed = true;
        foreach (var scope in _scopes.Values)
        {
            scope.Dispose();
        }

        _scopes.Clear();
    }

    public async ValueTask DisposeAsync()
    {
        _disposed = true;
        foreach (var id in _scopes.Keys.ToArray())
        {
            await CloseAsync(id);
        }
    }
}
