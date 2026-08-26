using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MiraiSpace.Extensibility.Abstractions.Modules;
using MiraiSpace.Presentation.Menu.Demo;
using MiraiSpace.Presentation.ViewModels;
using MiraiSpace.UI.Views;

namespace MiraiSpace.UI;

public partial class App : Application
{
    private IHost? _host;

    internal static IServiceProvider Services =>
        ((App)Current!)._host?.Services
        ?? throw new InvalidOperationException("Application services have not been initialized.");

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);

    }

    public override void OnFrameworkInitializationCompleted()
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();
        builder.Services.AddModule<AppMenuModule>();
        _host = builder.Build();
        _host.Start();

        MainViewModel mainViewModel = _host.Services.GetRequiredService<MainViewModel>();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = mainViewModel
            };
            desktop.Exit += (_, _) => StopHost();
        }
        else if (ApplicationLifetime is IActivityApplicationLifetime singleViewFactoryApplicationLifetime)
        {
            singleViewFactoryApplicationLifetime.MainViewFactory =
                () => new MainView { DataContext = mainViewModel };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = mainViewModel
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void StopHost()
    {
        _host?.StopAsync().GetAwaiter().GetResult();
        _host?.Dispose();
        _host = null;
    }
}
