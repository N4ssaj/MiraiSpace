using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Microsoft.Extensions.DependencyInjection;
using MiraiSpace.UI.Infrastructure;
using MiraiSpace.UI.Views;
using ReactiveMarbles.Extensions.Hosting.Avalonia;

namespace MiraiSpace.Desktop;

internal sealed class DesktopApplicationService : IAvaloniaService, IDisposable
{
    private readonly IServiceScopeFactory _scopeFactory;
    private IServiceScope? _applicationScope;

    public DesktopApplicationService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public void Initialize(Application application)
    {
        _applicationScope = _scopeFactory.CreateScope();
        var services = _applicationScope.ServiceProvider;

        application.DataTemplates.Add(services.GetRequiredService<ViewLocator>());

        if (application.ApplicationLifetime is not
            IClassicDesktopStyleApplicationLifetime desktop)
        {
            throw new InvalidOperationException(
                "MiraiSpace.Desktop requires a classic desktop application lifetime.");
        }

        desktop.MainWindow = services.GetRequiredService<MainWindow>();
        desktop.MainWindow.Show();
    }

    public void Dispose()
    {
        _applicationScope?.Dispose();
        _applicationScope = null;
    }
}
