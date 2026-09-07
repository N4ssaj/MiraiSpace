using Microsoft.Extensions.DependencyInjection;
using MiraiSpace.Extensibility.Abstractions.Menu;
using MiraiSpace.Presentation.CommandLine;
using MiraiSpace.Presentation.Navigation;
using MiraiSpace.UI.Views.Menu;
using ReactiveUI;

namespace MiraiSpace.Sample.Plugin.DependencyInjection;

public static class SampleServiceCollectionExtensions
{
    public static IServiceCollection AddSamplePlugin(this IServiceCollection services)
    {
        services.AddTransient<SamplePage>();
        services.AddTransient<IViewFor<SamplePage>, SamplePageView>();
        services.AddTransient<IViewFor<SampleMenuItem>, StandardAppMenuItemView<SampleMenuItem>>();
        services.AddKeyedScoped<IAppMenuItem, SampleMenuItem>(AppMenuKeys.Root);
        services.AddSingleton<NavigationRoute, SampleRoute>();
        services.AddScoped<ICommandModule, SampleCommands>();
        return services;
    }
}
