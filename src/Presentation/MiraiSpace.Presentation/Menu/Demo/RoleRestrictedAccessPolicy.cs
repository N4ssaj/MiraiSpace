using System.Reactive;
using MiraiSpace.Extensibility.Abstractions.Menu;

namespace MiraiSpace.Presentation.Menu.Demo;

public sealed class RoleRestrictedAccessPolicy(CurrentUserContext currentUser)
    : AppMenuItemAccessPolicy<IRoleRestricted>
{
    protected override bool CheckAccess(IRoleRestricted capability) =>
        capability.RequiredRoleIds.All(currentUser.HasRole);

    public override IObservable<Unit> AccessChanged => currentUser.RolesChanged;
}
