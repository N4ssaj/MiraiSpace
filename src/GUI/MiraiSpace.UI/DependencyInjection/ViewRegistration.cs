using Microsoft.Extensions.DependencyInjection;
using MiraiSpace.Presentation.Menu.Demo;
using MiraiSpace.UI.Views.Menu;
using ReactiveUI;

namespace MiraiSpace.UI.DependencyInjection;

public static class ViewRegistration
{
    public static IServiceCollection AddViews(this IServiceCollection services)
    {
        services.AddTransient<IViewFor<DashboardMenuItem>, MenuItemView<DashboardMenuItem>>();
        services.AddTransient<IViewFor<InboxMenuItem>, MenuItemView<InboxMenuItem>>();
        services.AddTransient<IViewFor<WorkspaceMenuItemContainer>, MenuItemView<WorkspaceMenuItemContainer>>();
        services.AddTransient<IViewFor<AdministrationMenuItem>, MenuItemView<AdministrationMenuItem>>();
        services.AddTransient<IViewFor<RoleToggleMenuItem>, MenuItemView<RoleToggleMenuItem>>();
        services.AddTransient<IViewFor<WorkspacePageMenuItem>, MenuItemView<WorkspacePageMenuItem>>();
        services.AddTransient<IViewFor<WorkspaceCalendarMenuItem>, MenuItemView<WorkspaceCalendarMenuItem>>();
        services.AddTransient<IViewFor<DelegateMenuItem>, MenuItemView<DelegateMenuItem>>();
        return services;
    }
}
