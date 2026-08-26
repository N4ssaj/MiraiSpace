using MiraiSpace.Extensibility.Abstractions.Menu;

namespace MiraiSpace.Presentation.Menu.Demo;

public sealed class WorkspacePageMenuItem(AppNavigationState navigation)
    : DemoMenuContribution(navigation, new(
        "workspace.pages", "workspace", 100, "Pages", "12 active", "▤", "#34A58B"))
{
    public override ValueTask ExecuteAsync(CancellationToken cancellationToken = default) =>
        NavigateAsync("WORKSPACE", "Pages", "Create, organize, and share knowledge with your team.");
}
