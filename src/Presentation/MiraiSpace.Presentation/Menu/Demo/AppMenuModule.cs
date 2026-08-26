using Microsoft.Extensions.DependencyInjection;
using MiraiSpace.Extensibility.Abstractions.Modules;
using MiraiSpace.Presentation.Abstractions.Menu;
using MiraiSpace.Presentation.Menu.Access;
using MiraiSpace.Presentation.Menu.Composition;
using MiraiSpace.Presentation.ViewModels;

namespace MiraiSpace.Presentation.Menu.Demo;

public sealed class AppMenuModule : IAppModule
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddLazyResolution();
        services.AddSingleton<AppNavigationState>();
        services.AddSingleton<CurrentUserContext>();
        services.AddSingleton<IAppMenuAccessPolicy, RoleRestrictedAccessPolicy>();
        services.AddSingleton<AppMenuAccessEvaluator>();
        services.AddSingleton<IAppMenuItemComparer, AppMenuItemComparer>();

        services.AddSingleton<IWorkspaceMenuItem, WorkspacePageMenuItem>();
        services.AddSingleton<IWorkspaceMenuItem, WorkspaceCalendarMenuItem>();
        services.AddSingleton<IWorkspaceMenuItem>(provider => new DelegateMenuItem(
            provider.GetRequiredService<AppNavigationState>(), "Maya Chen", "MC", "#D87A5D", 500));
        services.AddSingleton<IWorkspaceMenuItem>(provider => new DelegateMenuItem(
            provider.GetRequiredService<AppNavigationState>(), "Noah Wilson", "NW", "#557BC9", 600));

        services.AddSingleton<IAppMenuItem, DashboardMenuItem>();
        services.AddSingleton<IAppMenuItem, InboxMenuItem>();
        services.AddSingleton<IAppMenuItem, WorkspaceMenuItemContainer>();
        services.AddSingleton<IAppMenuItem, AdministrationMenuItem>();
        services.AddSingleton<IAppMenuItem, RoleToggleMenuItem>();

        services.AddSingleton<IAppMenu, AppMenuViewModel>();
        services.AddSingleton<MainViewModel>();
    }
}

public static class AppMenuModuleRegistration
{
    public static IServiceCollection AddDemoAppMenu(this IServiceCollection services) => services.AddModule<AppMenuModule>();
}
