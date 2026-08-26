using Microsoft.Extensions.DependencyInjection;
using MiraiSpace.Presentation.Menu.Demo;
using MiraiSpace.UI.Views.Menu;
using ReactiveUI;

namespace MiraiSpace.UI.DependencyInjection;

public static class ViewRegistration
{
    public static IServiceCollection AddViews(this IServiceCollection services)
    {
        services.AddStandardMenuView<DashboardMenuItem>();
        services.AddStandardMenuView<InboxMenuItem>();
        services.AddStandardMenuView<WorkspaceMenuItemContainer>();
        services.AddStandardMenuView<AdministrationMenuItem>();
        services.AddStandardMenuView<RoleToggleMenuItem>();
        services.AddStandardMenuView<WorkspacePageMenuItem>();
        services.AddStandardMenuView<WorkspaceCalendarMenuItem>();
        services.AddStandardMenuView<DelegateMenuItem>();
        return services;
    }

    private static IServiceCollection AddStandardMenuView<TViewModel>(this IServiceCollection services)
        where TViewModel : Presentation.Menu.Standard.StandardAppMenuItem =>
        services.AddTransient<IViewFor<TViewModel>, MenuItemView<TViewModel>>();
}
