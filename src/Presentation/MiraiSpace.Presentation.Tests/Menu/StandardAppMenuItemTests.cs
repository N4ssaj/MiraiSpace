using MiraiSpace.Extensibility.Abstractions.Menu;
using MiraiSpace.Presentation.Features.Workspace.Menu;
using MiraiSpace.Presentation.Features.Workspace.Navigation;

namespace MiraiSpace.Presentation.Tests.Menu;

public sealed class StandardAppMenuItemTests
{
    [Fact]
    public void ConcreteItemOwnsPublicSourceGeneratedCommand()
    {
        var navigation = new WorkspaceNavigationState();
        var item = new WorkspacePagesMenuItem(navigation);

        Assert.Same(item.ExecuteCommand, ((IAppMenuItem)item).ExecuteCommand);

        ((IAppMenuItem)item).ExecuteCommand.Execute(null);

        Assert.Equal("Pages", navigation.CurrentPage.Title);
    }
}
