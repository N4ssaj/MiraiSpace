using System.Windows.Input;
using MiraiSpace.Extensibility.Abstractions.Menu;
using MiraiSpace.Presentation.Features.Workspace.Navigation;
using MiraiSpace.Presentation.Menu.Standard;
using ReactiveUI.SourceGenerators;

namespace MiraiSpace.Presentation.Features.Workspace.Menu;

public sealed partial class WorkspaceCalendarMenuItem
    : StandardAppMenuItem, IAppMenuItem
{
    private readonly WorkspaceNavigationState _navigation;

    public override string Title => "Calendar";

    public override string Glyph => "□";

    public override string Accent => "#E7A84B";

    ICommand IAppMenuItem.ExecuteCommand => ExecuteCommand;

    public WorkspaceCalendarMenuItem(WorkspaceNavigationState navigation)
    {
        _navigation = navigation;
    }

    [ReactiveCommand]
    private void Execute()
    {
        _navigation.Navigate(
            "WORKSPACE",
            "Team calendar",
            "Plan milestones and keep everyone aligned.",
            Accent);
    }
}
