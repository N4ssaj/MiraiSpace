using MiraiSpace.Presentation.Menu.Demo;
using System.Reactive.Linq;

namespace MiraiSpace.Presentation.Tests.Menu;

public sealed class AppMenuItemCommandTests
{
    [Fact]
    public async Task CommandExecutesTheMenuItemAction()
    {
        var navigation = new AppNavigationState();
        using var item = new DashboardMenuItem(navigation);
        using IDisposable activation = item.Activator.Activate();

        await item.Command.Execute().FirstAsync();

        Assert.Equal("Good morning, Alex", navigation.Title);
    }
}
