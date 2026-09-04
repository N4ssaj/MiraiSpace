using Avalonia;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MiraiSpace.Presentation.DependencyInjection;
using MiraiSpace.UI;
using MiraiSpace.UI.DependencyInjection;
using ReactiveMarbles.Extensions.Hosting.Avalonia;
using ReactiveUI.Avalonia;

namespace MiraiSpace.Desktop;

internal sealed class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        builder.Services
            .AddPresentation()
            .AddMiraiSpaceUi();

        builder.ConfigureAvalonia(avalonia =>
        {
            avalonia.UseApplication<App>();
            avalonia.ConfigureAppBuilder(appBuilder =>
                appBuilder
                    .UsePlatformDetect()
                    .WithInterFont()
                    .UseReactiveUI(_ => { }));
        });
        builder.Services.AddSingleton<IAvaloniaService, DesktopApplicationService>();
        builder.UseAvaloniaLifetime();

        using var host = builder.Build();
        host.Run();
    }
}
