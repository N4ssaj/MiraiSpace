using Microsoft.Extensions.DependencyInjection;
using MiraiSpace.Extensibility.Abstractions.Menu;
using MiraiSpace.Presentation.Features.Workspace.Authorization;
using MiraiSpace.Presentation.Features.Workspace.Menu;
using MiraiSpace.Presentation.Features.Workspace.Navigation;
using MiraiSpace.Presentation.Navigation;
using MiraiSpace.Presentation.Session;

namespace MiraiSpace.Presentation.DependencyInjection;

internal static class WorkspaceServiceCollectionExtensions
{
    internal static IServiceCollection AddWorkspace(this IServiceCollection services)
    {
        services.AddScoped<CurrentUserContext>();
        services.AddScoped<IUserSession>(provider => provider.GetRequiredService<CurrentUserContext>());
        services.AddScoped<IAppMenuAccessPolicy, RoleRestrictedMenuAccessPolicy>();
        services.AddKeyedScoped<IAppMenuItem, OverviewMenuItem>(AppMenuKeys.Root);
        services.AddKeyedScoped<IAppMenuItem, InboxMenuItem>(AppMenuKeys.Root);
        services.AddKeyedScoped<IAppMenuItem, WorkspaceMenuItemContainer>(AppMenuKeys.Root);
        services.AddKeyedScoped<IAppMenuItem, AdministrationMenuItem>(AppMenuKeys.Root);
        services.AddKeyedScoped<IAppMenuItem, RoleToggleMenuItem>(AppMenuKeys.Root);
        services.AddKeyedScoped<IAppMenuItem, WorkspacePagesMenuItem>(AppMenuKeys.Workspace);
        services.AddKeyedScoped<IAppMenuItem, WorkspaceCalendarMenuItem>(AppMenuKeys.Workspace);
        services.AddTransient<WorkspacePageViewModel>();
        services.AddSingleton<NavigationRoute>(new WorkspaceRoute("/", WorkspaceSection.Overview));
        services.AddSingleton<NavigationRoute>(new WorkspaceRoute("/inbox", WorkspaceSection.Inbox));
        services.AddSingleton<NavigationRoute>(new WorkspaceRoute("/workspace/calendar", WorkspaceSection.Calendar));
        services.AddSingleton<NavigationRoute>(new WorkspaceRoute("/administration", WorkspaceSection.Administration));
        services.AddSingleton(new PageAccess("/administration", WorkspaceRoleIds.Administrator));
        return services;
    }
}
