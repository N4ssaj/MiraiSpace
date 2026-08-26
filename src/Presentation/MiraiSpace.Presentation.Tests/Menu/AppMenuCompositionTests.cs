using Microsoft.Extensions.DependencyInjection;
using MiraiSpace.Extensibility.Abstractions.Modules;
using MiraiSpace.Presentation.Abstractions.Menu;
using MiraiSpace.Presentation.Menu.Demo;
using ReactiveUI;

namespace MiraiSpace.Presentation.Tests.Menu;

public sealed class AppMenuCompositionTests
{
    [Fact]
    public void ModuleComposesRootsAndContainerOwnedChildren()
    {
        using ServiceProvider provider = new ServiceCollection()
            .AddModule<AppMenuModule>()
            .BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });
        IAppMenu menu = provider.GetRequiredService<IAppMenu>();
        using IDisposable activation = ((IActivatableViewModel)menu).Activator.Activate();

        IAppMenuItemContainer container = Assert.Single(menu.Items.OfType<IAppMenuItemContainer>());

        Assert.Collection(
            menu.Items,
            item => Assert.IsType<DashboardMenuItem>(item),
            item => Assert.IsType<InboxMenuItem>(item),
            item => Assert.IsType<WorkspaceMenuItemContainer>(item),
            item => Assert.IsType<RoleToggleMenuItem>(item));
        Assert.Collection(
            container.Items,
            item => Assert.IsType<WorkspacePageMenuItem>(item),
            item => Assert.IsType<WorkspaceCalendarMenuItem>(item),
            item => Assert.IsType<DelegateMenuItem>(item),
            item => Assert.IsType<DelegateMenuItem>(item));
    }

    [Fact]
    public void ActiveMenuReactsToRoleChanges()
    {
        using ServiceProvider provider = new ServiceCollection().AddModule<AppMenuModule>().BuildServiceProvider();
        IAppMenu menu = provider.GetRequiredService<IAppMenu>();
        using IDisposable activation = ((IActivatableViewModel)menu).Activator.Activate();

        provider.GetRequiredService<CurrentUserContext>().ToggleAdministrator();

        Assert.Contains(menu.Items, item => item is AdministrationMenuItem);
    }

    [Fact]
    public void LazyDependencyIsNotConstructedBeforeItIsRequested()
    {
        var services = new ServiceCollection();
        services.AddLazyResolution();
        services.AddTransient<LazyTarget>();
        using ServiceProvider provider = services.BuildServiceProvider();

        Lazy<LazyTarget> lazy = provider.GetRequiredService<Lazy<LazyTarget>>();

        Assert.False(lazy.IsValueCreated);
        Assert.IsType<LazyTarget>(lazy.Value);
        Assert.True(lazy.IsValueCreated);
    }

    private sealed class LazyTarget;
}
