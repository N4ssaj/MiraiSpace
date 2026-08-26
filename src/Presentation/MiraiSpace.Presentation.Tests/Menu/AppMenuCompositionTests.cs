using Microsoft.Extensions.DependencyInjection;
using MiraiSpace.Extensibility.Abstractions.Menu;
using MiraiSpace.Extensibility.Abstractions.Modules;
using MiraiSpace.Presentation.Menu;
using MiraiSpace.Presentation.Menu.Demo;
using ReactiveUI;

namespace MiraiSpace.Presentation.Tests.Menu;

public sealed class AppMenuCompositionTests
{
    [Fact]
    public void ModuleEntryPointComposesAFlatPresentationModel()
    {
        using ServiceProvider provider = new ServiceCollection()
            .AddModule<AppMenuModule>()
            .BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });

        IAppMenuViewModel menu = provider.GetRequiredService<IAppMenuViewModel>();

        Assert.Equal(
            ["overview", "inbox", "workspace", "workspace.pages", "workspace.calendar",
             "workspace.delegate.mc", "workspace.delegate.nw", "admin-mode"],
            menu.Items.Select(item => item.Id));
        Assert.DoesNotContain(menu.Items, item => item.Id == "administration");
    }

    [Fact]
    public void ActiveMenuRecomposesWhenAccessChanges()
    {
        using ServiceProvider provider = new ServiceCollection()
            .AddModule<AppMenuModule>()
            .BuildServiceProvider();
        var menu = (AppMenuViewModel)provider.GetRequiredService<IAppMenuViewModel>();
        using IDisposable activation = menu.Activator.Activate();

        provider.GetRequiredService<CurrentUserContext>().ToggleAdministrator();

        Assert.Contains(menu.Items, item => item.Id == "administration");
        Assert.Equal("Leave admin mode", menu.Items.Single(item => item.Id == "admin-mode").Title);
    }
}
