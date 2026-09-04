using Microsoft.Extensions.DependencyInjection;
using MiraiSpace.Presentation.Features.Workspace.Menu;
using MiraiSpace.Presentation.Features.Workspace.Navigation;
using MiraiSpace.Presentation.Menu;
using MiraiSpace.Presentation.ViewModels;
using MiraiSpace.UI.Views;
using MiraiSpace.UI.Views.Menu;
using ReactiveUI;
using ViewLocator = MiraiSpace.UI.Infrastructure.ViewLocator;

namespace MiraiSpace.UI.DependencyInjection;

public static class UiServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddMiraiSpaceUi()
        {
            ArgumentNullException.ThrowIfNull(services);

            services.AddScoped<ViewLocator>();
            services.AddScoped(provider => new MainWindow
            {
                ViewModel = provider.GetRequiredService<MainWindowViewModel>()
            });
            services.AddTransient<IViewFor<MainViewModel>, MainView>();
            services.AddTransient<IViewFor<AppMenuViewModel>, AppMenuView>();
            services.AddTransient<IViewFor<WorkspacePageViewModel>, WorkspacePageView>();
            services.AddTransient<IViewFor<OverviewMenuItem>, StandardAppMenuItemView<OverviewMenuItem>>();
            services.AddTransient<IViewFor<InboxMenuItem>, InboxMenuItemView>();
            services.AddTransient<IViewFor<WorkspaceMenuItemContainer>, WorkspaceMenuItemContainerView>();
            services.AddTransient<IViewFor<AdministrationMenuItem>, StandardAppMenuItemView<AdministrationMenuItem>>();
            services.AddTransient<IViewFor<RoleToggleMenuItem>, StandardAppMenuItemView<RoleToggleMenuItem>>();
            services.AddTransient<IViewFor<WorkspacePagesMenuItem>, StandardAppMenuItemView<WorkspacePagesMenuItem>>();
            services.AddTransient<IViewFor<WorkspaceCalendarMenuItem>, StandardAppMenuItemView<WorkspaceCalendarMenuItem>>();
            return services;
        }
    }
}
