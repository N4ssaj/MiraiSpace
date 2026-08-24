using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using MiraiSpace.Presentation.Menu.Demo;
using MiraiSpace.Presentation.ViewModels;
using MiraiSpace.UI.Views;

namespace MiraiSpace.UI;

public partial class App : Application
{
    private ServiceProvider? _services;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);

    }

    public override void OnFrameworkInitializationCompleted()
    {
        _services = new ServiceCollection()
            .AddDemoAppMenu()
            .AddMenuItemViews()
            .BuildServiceProvider(new ServiceProviderOptions
            {
                ValidateOnBuild = true,
                ValidateScopes = true
            });

        MainViewModel mainViewModel = _services.GetRequiredService<MainViewModel>();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = _services.GetRequiredService<MainWindow>();
            desktop.MainWindow.DataContext = mainViewModel;
            desktop.Exit += (_, _) => _services.Dispose();
        }
        else if (ApplicationLifetime is IActivityApplicationLifetime singleViewFactoryApplicationLifetime)
        {
            singleViewFactoryApplicationLifetime.MainViewFactory =
                () => CreateMainView(mainViewModel);
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = CreateMainView(mainViewModel);
        }

        base.OnFrameworkInitializationCompleted();
    }

    private MainView CreateMainView(MainViewModel viewModel)
    {
        MainView view = _services!.GetRequiredService<MainView>();
        view.DataContext = viewModel;
        return view;
    }
}
