using System.Reactive;
using MiraiSpace.Extensibility.Abstractions.Authorization;
using MiraiSpace.Extensibility.Abstractions.Menu;
using MiraiSpace.Presentation.Menu.Access;

namespace MiraiSpace.Presentation.Features.Workspace.Authorization;

public sealed class RoleRestrictedMenuAccessPolicy
    : AppMenuAccessPolicy<IRoleRestricted>
{
    private readonly CurrentUserContext _currentUser;

    public override IObservable<Unit> Invalidated => _currentUser.RolesChanged;

    public RoleRestrictedMenuAccessPolicy(CurrentUserContext currentUser)
    {
        _currentUser = currentUser;
    }

    protected override bool CanAccess(IRoleRestricted item) =>
        item.RequiredRoleIds.All(_currentUser.HasRole);
}
