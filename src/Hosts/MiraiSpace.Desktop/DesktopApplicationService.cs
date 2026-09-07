using System.Reactive;
using System.Reactive.Subjects;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MiraiSpace.Presentation.Dialogs;
using MiraiSpace.Presentation.Navigation;
using MiraiSpace.Presentation.Diagnostics;
using MiraiSpace.Presentation.Panels;
using MiraiSpace.Presentation.ViewModels;
using MiraiSpace.UI.Views;
using ReactiveMarbles.Extensions.Hosting.Avalonia;
using ReactiveUI;
using AvaloniaApplication = Avalonia.Application;
using ViewLocator = MiraiSpace.UI.Infrastructure.ViewLocator;

namespace MiraiSpace.Desktop;

internal sealed class DesktopApplicationService : IAvaloniaService, IHostedService, IDisposable
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly Subject<Exception> _commandErrors;
    private readonly ILogger<DesktopApplicationService> _logger;
    private readonly IPanelService _panels;
    private readonly CancellationTokenSource _lifetime = new();
    private IDisposable? _commandErrorSubscription;
    private AsyncServiceScope? _applicationScope;
    private AvaloniaApplication? _application;
    private MainWindow? _window;
    private ViewLocator? _viewLocator;
    private ReactiveCommand<Unit, Unit>? _startup;
    private Task? _initialization;
    private Task? _closing;
    private bool _allowClose;
    private bool _disposed;

    public DesktopApplicationService(
        IServiceScopeFactory scopeFactory,
        Subject<Exception> commandErrors,
        ILogger<DesktopApplicationService> logger,
        IPanelService panels)
    {
        _scopeFactory = scopeFactory;
        _commandErrors = commandErrors;
        _logger = logger;
        _panels = panels;
    }

    public void Initialize(AvaloniaApplication application)
    {
        if (application.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
        {
            throw new InvalidOperationException("MiraiSpace requires a classic desktop application lifetime.");
        }

        _application = application;
        _applicationScope = _scopeFactory.CreateAsyncScope();
        var services = _applicationScope.Value.ServiceProvider;
        _commandErrorSubscription = _commandErrors.Subscribe(services.GetRequiredService<CommandErrorState>());
        _viewLocator = services.GetRequiredService<ViewLocator>();
        application.DataTemplates.Add(_viewLocator);

        _window = services.GetRequiredService<MainWindow>();
        _window.Closing += OnWindowClosing;
        desktop.ShutdownMode = ShutdownMode.OnExplicitShutdown;
        desktop.MainWindow = _window;
        _window.Show();

        var navigation = services.GetRequiredService<INavigationService>();
        _startup = ReactiveCommand.CreateFromTask(() =>
            _initialization = navigation.NavigateAsync("/", _lifetime.Token));
        ((ICommand)_startup).Execute(null);
    }

    private void OnWindowClosing(object? sender, WindowClosingEventArgs args)
    {
        if (_allowClose)
        {
            return;
        }

        args.Cancel = true;
        _closing ??= CloseAsync();
    }

    public Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_applicationScope is null)
        {
            return;
        }

        if (Dispatcher.UIThread.CheckAccess())
        {
            await (_closing ??= CloseAsync());
        }
        else
        {
            await Dispatcher.UIThread.InvokeAsync(() => _closing ??= CloseAsync());
        }
    }

    private async Task CloseAsync()
    {
        // Finish the native Closing event before closing the window again after cleanup.
        await Task.Yield();
        try
        {
            _lifetime.Cancel();
            if (_panels is IAsyncDisposable panels)
            {
                await panels.DisposeAsync();
            }
            if (_applicationScope is { } scope)
            {
                var services = scope.ServiceProvider;
                var dialogs = services.GetRequiredService<IDialogService>();
                var navigation = services.GetRequiredService<INavigationService>();
                (dialogs as IDisposable)?.Dispose();
                (navigation as IDisposable)?.Dispose();

                if (_initialization is not null)
                {
                    await _initialization.ConfigureAwait(ConfigureAwaitOptions.ContinueOnCapturedContext | ConfigureAwaitOptions.SuppressThrowing);
                }

                if (dialogs is IAsyncDisposable asynchronousDialogs)
                {
                    await asynchronousDialogs.DisposeAsync();
                }

                if (navigation is IAsyncDisposable asynchronousNavigation)
                {
                    await asynchronousNavigation.DisposeAsync();
                }

                await scope.DisposeAsync();
                _applicationScope = null;
            }
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to release the application scope");
        }
        finally
        {
            if (_viewLocator is not null)
            {
                _application?.DataTemplates.Remove(_viewLocator);
            }

            _commandErrorSubscription?.Dispose();
            _commandErrorSubscription = null;
            _allowClose = true;
            _window?.Close();
            if (_application?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.Shutdown();
            }
        }
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        _lifetime.Cancel();
        _commandErrorSubscription?.Dispose();
        _lifetime.Dispose();
        if (_window is not null)
        {
            _window.Closing -= OnWindowClosing;
        }
    }
}
