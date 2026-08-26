using MiraiSpace.Presentation.Menu.Standard;

namespace MiraiSpace.Presentation.Menu.Demo;

public sealed class InboxMenuItem(AppNavigationState navigation) : StandardAppMenuItem(200)
{
    public override string Title => "Inbox";
    public override string Caption => "Messages and mentions";
    public override string Glyph => "✉";
    public override string Accent => "#ED6A5A";
    public override string Badge => "8";

    protected override Task ExecuteAsync(CancellationToken cancellationToken)
    {
        navigation.Navigate("COMMUNICATION", "Inbox", "Eight conversations are waiting for your attention.", Accent);
        return Task.CompletedTask;
    }
}
