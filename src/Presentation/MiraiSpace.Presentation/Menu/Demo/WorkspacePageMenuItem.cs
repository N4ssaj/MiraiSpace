namespace MiraiSpace.Presentation.Menu.Demo;

public sealed class WorkspacePageMenuItem(AppNavigationState navigation)
    : MenuItemViewModel(navigation, 100)
{
    public string Title => "Pages";

    public string Hint => "12 active";

    public override string DisplayTitle => Title;

    public override string Caption => Hint;

    public override string Glyph => "▤";

    public override string Accent => "#34A58B";

    public override ValueTask ExecuteAsync(CancellationToken cancellationToken = default)
    {
        Navigation.Navigate(
            "WORKSPACE",
            "Pages",
            "Create, organize, and share knowledge with your team.",
            "#34A58B");
        return ValueTask.CompletedTask;
    }
}
