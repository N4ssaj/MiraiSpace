using Microsoft.Extensions.DependencyInjection;
using MiraiSpace.Extensibility.Abstractions.Menu;
using MiraiSpace.Presentation.Features.Workspace.Authorization;
using MiraiSpace.Presentation.Features.Workspace.Menu;
using MiraiSpace.Presentation.Features.Workspace.Navigation;
using MiraiSpace.Presentation.Menu;
using MiraiSpace.Presentation.ViewModels;

namespace MiraiSpace.Presentation.DependencyInjection;

public static class PresentationServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddPresentation()
        {
            ArgumentNullException.ThrowIfNull(services);

            services.AddScoped<WorkspaceNavigationState>();
            services.AddScoped<CurrentUserContext>();
            services.AddScoped<IAppMenuAccessPolicy, RoleRestrictedMenuAccessPolicy>();

            services.AddScoped<AppMenuViewModel>();
            services.AddScoped<MainViewModel>();
            services.AddScoped<MainWindowViewModel>();

            services.AddKeyedScoped<IAppMenuItem, OverviewMenuItem>(AppMenuKeys.Root);
            services.AddKeyedScoped<IAppMenuItem, InboxMenuItem>(AppMenuKeys.Root);
            services.AddKeyedScoped<IAppMenuItem, WorkspaceMenuItemContainer>(AppMenuKeys.Root);
            services.AddKeyedScoped<IAppMenuItem, AdministrationMenuItem>(AppMenuKeys.Root);
            services.AddKeyedScoped<IAppMenuItem, RoleToggleMenuItem>(AppMenuKeys.Root);

            services.AddKeyedScoped<IAppMenuItem, WorkspacePagesMenuItem>(AppMenuKeys.Workspace);
            services.AddKeyedScoped<IAppMenuItem, WorkspaceCalendarMenuItem>(AppMenuKeys.Workspace);

            return services;
        }
    }
}
