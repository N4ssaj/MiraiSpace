using Microsoft.Extensions.DependencyInjection;
using MiraiSpace.Extensibility.Abstractions.Modules;
using MiraiSpace.Presentation.Abstractions.Menu;
using MiraiSpace.Presentation.Menu.Demo;
using ReactiveUI;

namespace MiraiSpace.Presentation.Tests.Menu;

public sealed class AppMenuCompositionTests
{
    [Fact]
    public void ModuleComposesRootsAndKeepsContainerOwnership()
    {
        using ServiceProvider provider = new ServiceCollection()
            .AddModule<AppMenuModule>()
            .BuildServiceProvider(new ServiceProviderOptions
            {
                ValidateOnBuild = true,
                ValidateScopes = true
            });

        IAppMenu menu = provider.GetRequiredService<IAppMenu>();
        var workspace = Assert.Single(menu.Items.OfType<WorkspaceMenuItemContainer>());

        Assert.Contains(menu.Items, item => item is DashboardMenuItem);
        Assert.DoesNotContain(menu.Items, item => item is AdministrationMenuItem);
        Assert.Empty(workspace.Items);

        using IDisposable activation = workspace.Activator.Activate();

        Assert.Collection(
            workspace.Items,
            item => Assert.IsType<WorkspacePageMenuItem>(item),
            item => Assert.IsType<WorkspaceCalendarMenuItem>(item),
            item => Assert.IsType<DelegateMenuItem>(item),
            item => Assert.IsType<DelegateMenuItem>(item));
    }

    [Fact]
    public void RoleChangeRevealsRestrictedRootItem()
    {
        using ServiceProvider provider = new ServiceCollection()
            .AddModule<AppMenuModule>()
            .BuildServiceProvider();
        IAppMenu menu = provider.GetRequiredService<IAppMenu>();

        provider.GetRequiredService<CurrentUserContext>().ToggleAdministrator();

        Assert.Contains(menu.Items, item => item is AdministrationMenuItem);
    }
}
