using Microsoft.Extensions.DependencyInjection;
using MiraiSpace.Extensibility.Abstractions.Menu;
using MiraiSpace.Presentation.ViewModels;

namespace MiraiSpace.Presentation.Menu.Demo;

public static class AppMenuModule
{
    public static IServiceCollection AddDemoAppMenu(this IServiceCollection services)
    {
        services.AddSingleton<AppNavigationState>();
        services.AddSingleton<CurrentUserContext>();

        services.AddSingleton<IAppMenuItemAccessPolicy, RoleRestrictedAccessPolicy>();
        services.AddSingleton<IAppMenuItemAccessChecker, AppMenuItemAccessChecker>();
        services.AddSingleton<IAppMenuItemExecutor, AppMenuItemExecutor>();

        services.AddKeyedSingleton<IAppMenuItem, DashboardMenuItem>(AppMenuKeys.Root);
        services.AddKeyedSingleton<IAppMenuItem, InboxMenuItem>(AppMenuKeys.Root);
        services.AddKeyedSingleton<IAppMenuItem, WorkspaceMenuItemContainer>(AppMenuKeys.Root);
        services.AddKeyedSingleton<IAppMenuItem, AdministrationMenuItem>(AppMenuKeys.Root);
        services.AddKeyedSingleton<IAppMenuItem, RoleToggleMenuItem>(AppMenuKeys.Root);
        services.AddKeyedSingleton<IAppMenuItem, WorkspacePageMenuItem>(AppMenuKeys.Workspace);
        services.AddKeyedSingleton<IAppMenuItem, WorkspaceCalendarMenuItem>(AppMenuKeys.Workspace);

        services.AddSingleton<IAppMenuViewModel, AppMenuViewModel>();
        services.AddSingleton<MainViewModel>();

        return services;
    }
}
