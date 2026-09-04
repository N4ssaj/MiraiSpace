using System.Windows.Input;
using MiraiSpace.Extensibility.Abstractions.Menu;
using MiraiSpace.Presentation.Features.Workspace.Navigation;
using MiraiSpace.Presentation.Menu.Standard;
using ReactiveUI.SourceGenerators;

namespace MiraiSpace.Presentation.Features.Workspace.Menu;

public sealed partial class WorkspacePagesMenuItem
    : StandardAppMenuItem, IAppMenuItem
{
    private readonly WorkspaceNavigationState _navigation;

    public override string Title => "Pages";

    public override string Caption => "12 active";

    public override string Glyph => "▤";

    public override string Accent => "#34A58B";

    ICommand IAppMenuItem.ExecuteCommand => ExecuteCommand;

    public WorkspacePagesMenuItem(WorkspaceNavigationState navigation)
    {
        _navigation = navigation;
    }

    [ReactiveCommand]
    private void Execute()
    {
        _navigation.Navigate(
            "WORKSPACE",
            "Pages",
            "Create, organize, and share knowledge with your team.",
            Accent);
    }
}
