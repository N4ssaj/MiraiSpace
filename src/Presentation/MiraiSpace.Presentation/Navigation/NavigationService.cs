using System.Reactive.Linq;
using System.Windows.Input;
using ReactiveUI;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using MiraiSpace.Presentation.Foundation;
using MiraiSpace.Presentation.Lifecycle;
using ReactiveUI.SourceGenerators;

namespace MiraiSpace.Presentation.Navigation;

public sealed partial class NavigationService : ReactiveModel, INavigationService, IDisposable, IAsyncDisposable
{
    private readonly IServiceProvider _services;
    private readonly NavigationRouter _router;
    private readonly INavigationHost _host;
    private readonly NavigationAccess _access;
    private readonly SemaphoreSlim _gate = new(1);
    private readonly CancellationTokenSource _lifetime = new();
    private readonly ConditionalWeakTable<ReactivePage, NavigationAddress> _addresses = new();
    private readonly IDisposable _accessSubscription;
    private Task? _shutdown;
    private bool _disposed;

    [Reactive]
    public partial ReactivePage? Current { get; private set; }

    [Reactive]
    public partial NavigationAddress? Address { get; private set; }

    [Reactive]
    public partial bool IsNavigating { get; private set; }

    public NavigationService(
        IServiceProvider services,
        NavigationRouter router,
        INavigationHost host,
        NavigationAccess access)
    {
        _services = services;
        _router = router;
        _host = host;
        _access = access;
        var checkAccess = ReactiveCommand.CreateFromTask(async () =>
        {
            if (Current is not null && !_access.IsAllowed(Address))
            {
                await NavigateAsync("/", _lifetime.Token);
            }
        });
        _accessSubscription = access.Invalidated.ObserveOn(RxSchedulers.MainThreadScheduler)
            .Subscribe(_ => ((ICommand)checkAccess).Execute(null));
    }

    public Task NavigateAsync(string address, CancellationToken cancellationToken = default)
    {
        var location = NavigationAddress.Parse(address);
        var route = _router.Resolve(location);
        return PushAsync(
            route.PageType,
            location,
            (page, token) => route.InitializeAsync(page, location, token),
            cancellationToken);
    }

    public Task NavigateAsync<TPage>(CancellationToken cancellationToken = default)
        where TPage : ReactivePage =>
        PushAsync(
            typeof(TPage),
            _router.FindAddress(typeof(TPage)),
            (page, token) => page is IInitializable initializable
                ? initializable.InitializeAsync(token)
                : ValueTask.CompletedTask,
            cancellationToken);

    public Task NavigateAsync<TPage, TParameters>(
        TParameters parameters,
        CancellationToken cancellationToken = default)
        where TPage : ReactivePage, IInitializable<TParameters> =>
        PushAsync(
            typeof(TPage),
            null,
            (page, token) => ((TPage)page).InitializeAsync(parameters, token),
            cancellationToken);

    private async Task PushAsync(
        Type pageType,
        NavigationAddress? address,
        Func<ReactivePage, CancellationToken, ValueTask> initialize,
        CancellationToken cancellationToken)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _lifetime.Token);
        await _gate.WaitAsync(linked.Token);
        try
        {
            _access.EnsureAllowed(address);
            IsNavigating = true;
            var page = (ReactivePage)_services.GetRequiredService(pageType);
            await initialize(page, linked.Token);
            _access.EnsureAllowed(address);
            await _host.PushAsync(page, linked.Token);
            if (address is not null)
            {
                _addresses.Add(page, address);
            }

            UpdateCurrent();
        }
        finally
        {
            IsNavigating = false;
            _gate.Release();
        }
    }

    public async Task GoBackAsync(CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _lifetime.Token);
        await _gate.WaitAsync(linked.Token);
        try
        {
            IsNavigating = true;
            do
            {
                await _host.PopAsync(linked.Token);
            }
            while (_host.Count > 1 && _host.Current is { } page && !_access.IsAllowed(_addresses.TryGetValue(page, out var location) ? location : null));

            UpdateCurrent();
        }
        finally
        {
            IsNavigating = false;
            _gate.Release();
        }
    }

    private void UpdateCurrent()
    {
        Current = _host.Current;
        Address = Current is not null && _addresses.TryGetValue(Current, out var address) ? address : null;
    }

    public void Dispose() => CancelPending();

    private void CancelPending()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        _lifetime.Cancel();
        _accessSubscription.Dispose();
    }
    public ValueTask DisposeAsync() => new(_shutdown ??= StopAsync());

    private async Task StopAsync()
    {
        CancelPending();
        await _gate.WaitAsync();
        _gate.Dispose();
        _lifetime.Dispose();
    }
}
