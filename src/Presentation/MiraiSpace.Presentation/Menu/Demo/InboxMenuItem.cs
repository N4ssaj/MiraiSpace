using MiraiSpace.Extensibility.Abstractions.Menu;

namespace MiraiSpace.Presentation.Menu.Demo;

public sealed class InboxMenuItem(AppNavigationState navigation)
    : DemoMenuContribution(navigation, new(
        "inbox", null, 200, "Inbox", "Messages and mentions", "✉", "#ED6A5A", "8"))
{
    public override ValueTask ExecuteAsync(CancellationToken cancellationToken = default) =>
        NavigateAsync(
            "COMMUNICATION",
            "Inbox",
            "Eight conversations are waiting for your attention.");
}
