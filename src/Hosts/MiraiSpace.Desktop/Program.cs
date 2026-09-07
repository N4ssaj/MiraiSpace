using System.Reactive.Subjects;
using Avalonia;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReactiveMarbles.Extensions.Hosting.Plugins;
using MiraiSpace.UI;
using MiraiSpace.Desktop.DependencyInjection;
using Serilog;
using ReactiveMarbles.Extensions.Hosting.Avalonia;
using ReactiveUI.Avalonia;

namespace MiraiSpace.Desktop;

internal static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        using var commandErrors = new Subject<Exception>();
        var builder = Host.CreateApplicationBuilder(args);
        builder.Services.AddSerilog(configuration => configuration
            .Enrich.FromLogContext()
            .WriteTo.Console());
        builder.Services.AddSingleton(commandErrors);
        builder.Services.AddDesktopApplication();

        builder.ConfigurePlugins(plugins =>
        {
            plugins.AddScanDirectories(AppContext.BaseDirectory);
            plugins.IncludePlugins("plugins/**/*.Plugin.dll");
            plugins.AssemblyScanFunc = PluginScanner.ScanForPluginInstances;
        });

        builder.ConfigureAvalonia(avalonia =>
        {
            avalonia.UseApplication<App>();
            avalonia.ConfigureAppBuilder(appBuilder =>
                appBuilder
                    .UsePlatformDetect()
                    .WithInterFont()
                    .UseReactiveUI(reactiveUi => reactiveUi.WithExceptionHandler(commandErrors)));
        });
        builder.Services.AddSingleton<DesktopApplicationService>();
        builder.Services.AddSingleton<IAvaloniaService>(services => services.GetRequiredService<DesktopApplicationService>());
        builder.UseAvaloniaLifetime();
        builder.Services.AddHostedService(services => services.GetRequiredService<DesktopApplicationService>());

        using var host = builder.Build();
        host.Run();
    }
}
