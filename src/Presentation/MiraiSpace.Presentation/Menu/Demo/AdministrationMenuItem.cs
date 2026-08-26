using MiraiSpace.Extensibility.Abstractions.Common;
using MiraiSpace.Extensibility.Abstractions.Menu;

namespace MiraiSpace.Presentation.Menu.Demo;

public sealed class AdministrationMenuItem(AppNavigationState navigation)
    : DemoMenuContribution(navigation, new(
        "administration", null, 400, "Administration", "Roles & policies", "⚙", "#C267E7")),
        IRoleRestricted
{
    public IReadOnlyCollection<Guid> RequiredRoleIds { get; } = [RoleIds.Administrator];

    public override ValueTask ExecuteAsync(CancellationToken cancellationToken = default) =>
        NavigateAsync(
            "ADMINISTRATION",
            "Access management",
            "Manage roles, permissions, and workspace policies.");
}
