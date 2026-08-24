namespace MiraiSpace.Presentation.Menu.Demo;

public sealed class InboxMenuItem(AppNavigationState navigation)
    : MenuItemViewModel(navigation, 200)
{
    public string Title => "Inbox";

    public int UnreadCount => 8;

    public override string DisplayTitle => Title;

    public override string Glyph => "✉";

    public override string Accent => "#ED6A5A";

    public override string Badge => UnreadCount.ToString();

    public override ValueTask ExecuteAsync(CancellationToken cancellationToken = default)
    {
        Navigation.Navigate(
            "COMMUNICATION",
            "Inbox",
            "Eight conversations are waiting for your attention.",
            "#ED6A5A");
        return ValueTask.CompletedTask;
    }
}
