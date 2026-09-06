using System.Windows.Input;
using MiraiSpace.Extensibility.Abstractions.Authorization;
using MiraiSpace.Extensibility.Abstractions.Menu;
using MiraiSpace.Presentation.Features.Workspace.Authorization;
using MiraiSpace.Presentation.Features.Workspace.Navigation;
using MiraiSpace.Presentation.Menu.Standard;
using ReactiveUI.SourceGenerators;

namespace MiraiSpace.Presentation.Features.Workspace.Menu;

public sealed partial class AdministrationMenuItem
    : StandardAppMenuItem, IAppMenuItem, IRoleRestricted
{
    private readonly WorkspaceNavigationState _navigation;

    public override string Title => "Administration";

    public override string Caption => "Roles & policies";

    public override string Glyph => "⚙";

    public override string Accent => "#C267E7";

    public IReadOnlyList<Guid> RequiredRoleIds { get; } =
        [WorkspaceRoleIds.Administrator];

    ICommand IAppMenuItem.ExecuteCommand => ExecuteCommand;

    public AdministrationMenuItem(WorkspaceNavigationState navigation)
    {
        _navigation = navigation;
    }

    [ReactiveCommand]
    private void Execute()
    {
        _navigation.Navigate(
            "ADMINISTRATION",
            "Access management",
            "Manage roles, permissions, and workspace policies.",
            Accent);
    }
}
