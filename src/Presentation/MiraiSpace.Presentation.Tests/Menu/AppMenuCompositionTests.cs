using Microsoft.Extensions.DependencyInjection;
using MiraiSpace.Extensibility.Abstractions.Menu;
using MiraiSpace.Presentation.Menu;
using MiraiSpace.Presentation.Menu.Demo;

namespace MiraiSpace.Presentation.Tests.Menu;

public sealed class AppMenuCompositionTests
{
    [Fact]
    public void KeyedRegistrationsComposeRootAndContainerItems()
    {
        using ServiceProvider provider = new ServiceCollection()
            .AddDemoAppMenu()
            .BuildServiceProvider(new ServiceProviderOptions
            {
                ValidateOnBuild = true,
                ValidateScopes = true
            });

        IAppMenuViewModel menu = provider.GetRequiredService<IAppMenuViewModel>();
        IAppMenuItemContainer container = Assert.Single(menu.Containers);

        Assert.Contains(menu.Items, x => x is DashboardMenuItem);
        Assert.DoesNotContain(menu.Items, x => x is AdministrationMenuItem);
        Assert.Collection(
            container.Items,
            x => Assert.IsType<WorkspacePageMenuItem>(x),
            x => Assert.IsType<WorkspaceCalendarMenuItem>(x),
            x => Assert.IsType<DelegateMenuItem>(x),
            x => Assert.IsType<DelegateMenuItem>(x));
    }

    [Fact]
    public void RoleChangeRevealsRestrictedRootItem()
    {
        using ServiceProvider provider = new ServiceCollection()
            .AddDemoAppMenu()
            .BuildServiceProvider();
        IAppMenuViewModel menu = provider.GetRequiredService<IAppMenuViewModel>();

        provider.GetRequiredService<CurrentUserContext>().ToggleAdministrator();

        Assert.Contains(menu.Items, x => x is AdministrationMenuItem);
    }
}
