using System.Windows.Input;
using MiraiSpace.Extensibility.Abstractions.Menu;
using MiraiSpace.Presentation.Features.Workspace.Navigation;
using MiraiSpace.Presentation.Menu.Standard;
using ReactiveUI.SourceGenerators;

namespace MiraiSpace.Presentation.Features.Workspace.Menu;

public sealed partial class OverviewMenuItem : StandardAppMenuItem, IAppMenuItem
{
    private readonly WorkspaceNavigationState _navigation;

    public override string Title => "Overview";

    public override string Caption => "Your daily pulse";

    public override string Glyph => "⌂";

    ICommand IAppMenuItem.ExecuteCommand => ExecuteCommand;

    public OverviewMenuItem(WorkspaceNavigationState navigation)
    {
        _navigation = navigation;
    }

    [ReactiveCommand]
    private void Execute()
    {
        _navigation.Navigate(
            "OVERVIEW",
            "Good morning, Alex",
            "Here is what is happening across your workspace today.",
            Accent);
    }
}
