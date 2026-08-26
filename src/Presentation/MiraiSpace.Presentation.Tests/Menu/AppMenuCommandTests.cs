using MiraiSpace.Presentation.Menu.Demo;
using System.Reactive.Linq;

namespace MiraiSpace.Presentation.Tests.Menu;

public sealed class AppMenuCommandTests
{
    [Fact]
    public async Task ItemCommandExecutesItsNavigationAction()
    {
        var navigation = new AppNavigationState();
        var item = new InboxMenuItem(navigation);

        await item.Command.Execute();

        Assert.Equal("Inbox", navigation.Title);
    }
}
