namespace MiraiSpace.Presentation.Menu.Demo;

public sealed class WorkspaceCalendarMenuItem(AppNavigationState navigation)
    : MenuItemViewModel(navigation, "workspace.calendar", 200)
{
    public string Title => "Calendar";

    public override string DisplayTitle => Title;

    public override string Glyph => "□";

    public override string Accent => "#E7A84B";

    public override ValueTask ExecuteAsync(CancellationToken cancellationToken = default)
    {
        Navigation.Navigate(
            RouteKey,
            "WORKSPACE",
            "Team calendar",
            "Plan milestones and keep everyone aligned.",
            "#E7A84B");
        return ValueTask.CompletedTask;
    }
}
