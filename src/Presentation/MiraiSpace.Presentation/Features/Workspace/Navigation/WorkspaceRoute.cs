using MiraiSpace.Presentation.Navigation;

namespace MiraiSpace.Presentation.Features.Workspace.Navigation;

public sealed class WorkspaceRoute : NavigationRoute<WorkspacePageViewModel, WorkspaceSection>
{
    private readonly WorkspaceSection _section;

    public override string Path { get; }

    public WorkspaceRoute(string path, WorkspaceSection section)
    {
        Path = path;
        _section = section;
    }

    protected override WorkspaceSection ReadParameters(NavigationAddress address) => _section;
}
