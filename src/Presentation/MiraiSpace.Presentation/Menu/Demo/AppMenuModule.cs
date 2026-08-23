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

        services
            .AddAppMenuItem<DashboardMenuItem>(AppMenuKeys.Root)
            .AddAppMenuItem<InboxMenuItem>(AppMenuKeys.Root)
            .AddAppMenuItem<WorkspaceMenuItemContainer>(AppMenuKeys.Root)
            .AddAppMenuItem<AdministrationMenuItem>(AppMenuKeys.Root)
            .AddAppMenuItem<RoleToggleMenuItem>(AppMenuKeys.Root)
            .AddAppMenuItem<WorkspacePageMenuItem>(AppMenuKeys.Workspace)
            .AddAppMenuItem<WorkspaceCalendarMenuItem>(AppMenuKeys.Workspace);

        services.AddSingleton<IAppMenuViewModel, AppMenuViewModel>();
        services.AddSingleton<MainViewModel>();

        return services;
    }
}
