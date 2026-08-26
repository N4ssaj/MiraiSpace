using Microsoft.Extensions.DependencyInjection;
using MiraiSpace.Extensibility.Abstractions.Menu;
using MiraiSpace.Extensibility.Abstractions.Modules;
using MiraiSpace.Presentation.ViewModels;

namespace MiraiSpace.Presentation.Menu.Demo;

public sealed class AppMenuModule : IAppModule
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<AppNavigationState>();
        services.AddSingleton<CurrentUserContext>();

        services.AddSingleton<IAppMenuAccessPolicy, RoleRestrictedAccessPolicy>();
        services.AddSingleton<IAppMenuAccessEvaluator, AppMenuAccessEvaluator>();
        services.AddSingleton<IAppMenuContributionExecutor, AppMenuContributionExecutor>();

        services.AddSingleton<IAppMenuContribution, DashboardMenuItem>();
        services.AddSingleton<IAppMenuContribution, InboxMenuItem>();
        services.AddSingleton<IAppMenuContribution, WorkspaceMenuItem>();
        services.AddSingleton<IAppMenuContribution, AdministrationMenuItem>();
        services.AddSingleton<IAppMenuContribution, RoleToggleMenuItem>();
        services.AddSingleton<IAppMenuContribution, WorkspacePageMenuItem>();
        services.AddSingleton<IAppMenuContribution, WorkspaceCalendarMenuItem>();
        services.AddSingleton<IAppMenuContribution>(provider => new DelegateMenuItem(
            provider.GetRequiredService<AppNavigationState>(), "Maya Chen", "MC", "#D87A5D", 500));
        services.AddSingleton<IAppMenuContribution>(provider => new DelegateMenuItem(
            provider.GetRequiredService<AppNavigationState>(), "Noah Wilson", "NW", "#557BC9", 600));

        services.AddSingleton<IAppMenuViewModel, AppMenuViewModel>();
        services.AddSingleton<MainViewModel>();
    }
}

public static class AppMenuModuleRegistration
{
    public static IServiceCollection AddDemoAppMenu(this IServiceCollection services) =>
        services.AddModule<AppMenuModule>();
}
