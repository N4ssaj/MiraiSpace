using MiraiSpace.Presentation.Menu.Standard;

namespace MiraiSpace.Presentation.Menu.Demo;

public sealed class WorkspacePageMenuItem(AppNavigationState navigation) : StandardAppMenuItem(100), IWorkspaceMenuItem
{
    public override string Title => "Pages";
    public override string Caption => "12 active";
    public override string Glyph => "▤";
    public override string Accent => "#34A58B";

    protected override Task ExecuteAsync(CancellationToken cancellationToken)
    {
        navigation.Navigate("WORKSPACE", "Pages", "Create, organize, and share knowledge with your team.", Accent);
        return Task.CompletedTask;
    }
}
