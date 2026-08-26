using MiraiSpace.Extensibility.Abstractions.Menu;

namespace MiraiSpace.Presentation.Menu.Demo;

public sealed class WorkspaceMenuItem(AppNavigationState navigation)
    : DemoMenuContribution(navigation, new(
        "workspace", null, 300, "Workspace", "Team space", "◇", "#34A58B"))
{
    public override ValueTask ExecuteAsync(CancellationToken cancellationToken = default) =>
        NavigateAsync(
            "WORKSPACE",
            "Your workspace",
            "Everything your team is building, planning, and sharing.");
}
