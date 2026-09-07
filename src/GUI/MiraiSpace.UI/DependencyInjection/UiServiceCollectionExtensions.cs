using Microsoft.Extensions.DependencyInjection;
using MiraiSpace.Presentation.Dialogs;
using MiraiSpace.Presentation.Features.Documents;
using MiraiSpace.Presentation.Navigation;
using MiraiSpace.UI.Navigation;
using MiraiSpace.Presentation.Features.CommandConsole;
using MiraiSpace.Presentation.Foundation;
using MiraiSpace.Presentation.Panels;
using MiraiSpace.UI.Infrastructure;
using MiraiSpace.UI.Dialogs;
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
        public IServiceCollection AddUi()
        {
            ArgumentNullException.ThrowIfNull(services);

            services.AddSingleton<IUiDispatcher, UiDispatcher>();
            services.AddScoped<ViewLocator>();
            services.AddScoped<INavigationHost, AvaloniaNavigationHost>();
            services.AddScoped<IDialogService, AvaloniaDialogService>();
            services.AddTransient<IViewFor<DocumentsPageViewModel>, DocumentsPageView>();
            services.AddTransient<IViewFor<DocumentPageViewModel>, DocumentPageView>();
            services.AddTransient<IViewFor<DocumentEditorComponent>, DocumentEditorView>();
            services.AddTransient<IViewFor<DocumentDetailsComponent>, DocumentDetailsView>();
            services.AddTransient<IViewFor<DocumentNameComponent>, DocumentNameView>();
            services.AddTransient<IViewFor<RenameDocumentDialog>, RenameDocumentView>();
            services.AddTransient<IViewFor<DeleteDocumentDialog>, DeleteDocumentView>();
            services.AddScoped<MainWindow>();
            services.AddTransient<IViewFor<MainViewModel>, MainView>();
            services.AddTransient<IViewFor<CommandConsoleViewModel>, CommandConsoleView>();
            services.AddTransient<IViewFor<NavigationPanelViewModel>, NavigationPanelView>();
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
