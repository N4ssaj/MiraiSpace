using Microsoft.Extensions.DependencyInjection;
using MiraiSpace.Extensibility.Abstractions.Modules;
using MiraiSpace.Presentation.Abstractions.Menu;
using MiraiSpace.Presentation.DependencyInjection;
using MiraiSpace.Presentation.Menu.Access;
using MiraiSpace.Presentation.Menu.Composition;
using MiraiSpace.Presentation.ViewModels;

namespace MiraiSpace.Presentation.Menu.Demo;

public sealed class AppMenuModule : IAppModule
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<AppNavigationState>();
        services.AddSingleton<CurrentUserContext>();

        services.AddSingleton<IAppMenuItemAccessPolicy, RoleRestrictedAccessPolicy>();
        services.AddSingleton<IAppMenuItemAccess, AppMenuItemAccess>();
        services.AddSingleton<IComparer<IAppMenuItem>, AppMenuItemComparer>();

        services.AddSingleton<DashboardMenuItem>();
        services.AddSingleton<InboxMenuItem>();
        services.AddSingleton<WorkspaceMenuItemContainer>();
        services.AddSingleton<AdministrationMenuItem>();
        services.AddSingleton<RoleToggleMenuItem>();
        services.AddSingleton<WorkspacePageMenuItem>();
        services.AddSingleton<WorkspaceCalendarMenuItem>();

        services.AddKeyedSingleton<IAppMenuItem>(AppMenuKeys.Root,
            (provider, _) => provider.GetRequiredService<DashboardMenuItem>());
        services.AddKeyedSingleton<IAppMenuItem>(AppMenuKeys.Root,
            (provider, _) => provider.GetRequiredService<InboxMenuItem>());
        services.AddKeyedSingleton<IAppMenuItem>(AppMenuKeys.Root,
            (provider, _) => provider.GetRequiredService<WorkspaceMenuItemContainer>());
        services.AddKeyedSingleton<IAppMenuItem>(AppMenuKeys.Root,
            (provider, _) => provider.GetRequiredService<AdministrationMenuItem>());
        services.AddKeyedSingleton<IAppMenuItem>(AppMenuKeys.Root,
            (provider, _) => provider.GetRequiredService<RoleToggleMenuItem>());

        services.AddLazy<WorkspacePageMenuItem>();
        services.AddLazy<WorkspaceCalendarMenuItem>();
        services.AddSingleton(provider => new Lazy<IReadOnlyList<DelegateMenuItem>>(() =>
        [
            new DelegateMenuItem(
                provider.GetRequiredService<AppNavigationState>(),
                "Maya Chen", "MC", "#D87A5D", 500),
            new DelegateMenuItem(
                provider.GetRequiredService<AppNavigationState>(),
                "Noah Wilson", "NW", "#557BC9", 600)
        ]));

        services.AddSingleton<IAppMenu, AppMenu>();
        services.AddSingleton<MainViewModel>();
    }
}

public static class AppMenuModuleRegistration
{
    public static IServiceCollection AddDemoAppMenu(this IServiceCollection services) =>
        services.AddModule<AppMenuModule>();
}
