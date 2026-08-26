using MiraiSpace.Extensibility.Abstractions.Common;
using MiraiSpace.Presentation.Menu.Standard;

namespace MiraiSpace.Presentation.Menu.Demo;

public sealed class AdministrationMenuItem(AppNavigationState navigation) : StandardAppMenuItem(400), IRoleRestricted
{
    public override string Title => "Administration";
    public override string Caption => "Roles & policies";
    public override string Glyph => "⚙";
    public override string Accent => "#C267E7";
    public IReadOnlyCollection<Guid> RequiredRoleIds { get; } = [RoleIds.Administrator];

    protected override Task ExecuteAsync(CancellationToken cancellationToken)
    {
        navigation.Navigate("ADMINISTRATION", "Access management", "Manage roles, permissions, and workspace policies.", Accent);
        return Task.CompletedTask;
    }
}
