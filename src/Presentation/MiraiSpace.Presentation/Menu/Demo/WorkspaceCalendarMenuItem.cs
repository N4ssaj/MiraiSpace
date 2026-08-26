using MiraiSpace.Extensibility.Abstractions.Menu;

namespace MiraiSpace.Presentation.Menu.Demo;

public sealed class WorkspaceCalendarMenuItem(AppNavigationState navigation)
    : DemoMenuContribution(navigation, new(
        "workspace.calendar", "workspace", 200, "Calendar", "Planning", "□", "#E7A84B"))
{
    public override ValueTask ExecuteAsync(CancellationToken cancellationToken = default) =>
        NavigateAsync("WORKSPACE", "Team calendar", "Plan milestones and keep everyone aligned.");
}
