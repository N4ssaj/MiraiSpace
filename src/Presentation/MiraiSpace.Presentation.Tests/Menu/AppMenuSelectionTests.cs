using Microsoft.Extensions.DependencyInjection;
using MiraiSpace.Extensibility.Abstractions.Modules;
using MiraiSpace.Presentation.Menu;
using MiraiSpace.Presentation.Menu.Demo;
using ReactiveUI;

namespace MiraiSpace.Presentation.Tests.Menu;

public sealed class AppMenuSelectionTests
{
    [Fact]
    public async Task SelectionTracksExactAndParentRoute()
    {
        using ServiceProvider provider = new ServiceCollection()
            .AddModule<AppMenuModule>()
            .BuildServiceProvider();
        var menu = (AppMenuViewModel)provider.GetRequiredService<IAppMenuViewModel>();
        using IDisposable activation = menu.Activator.Activate();
        AppMenuItemModel pages = menu.Items.Single(item => item.Id == "workspace.pages");

        await menu.ExecuteAsync(pages);

        Assert.True(menu.Items.Single(item => item.Id == "workspace").IsSelected);
        Assert.True(pages.IsSelected);
        Assert.Same(pages, menu.SelectedItem);
    }
}
