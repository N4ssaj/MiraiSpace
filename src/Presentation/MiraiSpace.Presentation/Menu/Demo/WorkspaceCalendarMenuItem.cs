using MiraiSpace.Presentation.Menu.Standard;

namespace MiraiSpace.Presentation.Menu.Demo;

public sealed class WorkspaceCalendarMenuItem(AppNavigationState navigation) : StandardAppMenuItem(200), IWorkspaceMenuItem
{
    public override string Title => "Calendar";
    public override string Caption => "Planning";
    public override string Glyph => "□";
    public override string Accent => "#E7A84B";

    protected override Task ExecuteAsync(CancellationToken cancellationToken)
    {
        navigation.Navigate("WORKSPACE", "Team calendar", "Plan milestones and keep everyone aligned.", Accent);
        return Task.CompletedTask;
    }
}
