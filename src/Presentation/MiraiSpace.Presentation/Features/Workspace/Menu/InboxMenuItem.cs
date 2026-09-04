using System.Windows.Input;
using MiraiSpace.Extensibility.Abstractions.Menu;
using MiraiSpace.Presentation.Features.Workspace.Navigation;
using MiraiSpace.Presentation.Menu.Standard;
using ReactiveUI.SourceGenerators;

namespace MiraiSpace.Presentation.Features.Workspace.Menu;

public sealed partial class InboxMenuItem : StandardAppMenuItem, IAppMenuItem
{
    private readonly WorkspaceNavigationState _navigation;

    public override string Title => "Inbox";

    public override string Glyph => "✉";

    public override string Accent => "#ED6A5A";

    public int UnreadCount { get; } = 8;

    ICommand IAppMenuItem.ExecuteCommand => ExecuteCommand;

    public InboxMenuItem(WorkspaceNavigationState navigation)
    {
        _navigation = navigation;
    }

    [ReactiveCommand]
    private void Execute()
    {
        _navigation.Navigate(
            "COMMUNICATION",
            "Inbox",
            "Eight conversations are waiting for your attention.",
            Accent);
    }
}
