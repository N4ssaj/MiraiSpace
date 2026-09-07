using MessagePipe;
using Microsoft.Extensions.DependencyInjection;
using MiraiSpace.Presentation.CommandLine;
using MiraiSpace.Presentation.Diagnostics;
using MiraiSpace.Presentation.Features.CommandConsole;
using MiraiSpace.Presentation.Menu;
using MiraiSpace.Presentation.Navigation;
using MiraiSpace.Presentation.Panels;
using MiraiSpace.Presentation.ViewModels;

namespace MiraiSpace.Presentation.DependencyInjection;

public static class PresentationServiceCollectionExtensions
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddMessagePipe(options => options.InstanceLifetime = InstanceLifetime.Singleton);
        services.AddScoped<CommandErrorState>();
        services.AddScoped<ICommandService, CommandService>();
        services.AddScoped<ICommandModule, NavigationCommands>();
        services.AddScoped<CommandConsoleViewModel>();
        services.AddSingleton<IPanelService, PanelService>();
        services.AddScoped<NavigationPanelViewModel>();
        services.AddSingleton<NavigationRouter>();
        services.AddScoped<NavigationAccess>();
        services.AddScoped<INavigationService, NavigationService>();
        services.AddScoped<AppMenuViewModel>();
        services.AddScoped<MainViewModel>();
        services.AddScoped<MainWindowViewModel>();
        return services.AddWorkspace().AddDocuments();
    }
}
