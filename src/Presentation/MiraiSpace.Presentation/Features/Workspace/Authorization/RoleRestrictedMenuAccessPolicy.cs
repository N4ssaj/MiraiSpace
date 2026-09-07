using MiraiSpace.Presentation.Session;
using System.Reactive;
using MiraiSpace.Extensibility.Abstractions.Authorization;
using MiraiSpace.Extensibility.Abstractions.Menu;
using MiraiSpace.Presentation.Menu.Access;

namespace MiraiSpace.Presentation.Features.Workspace.Authorization;

public sealed class RoleRestrictedMenuAccessPolicy
    : AppMenuAccessPolicy<IRoleRestricted>
{
    private readonly IUserSession _currentUser;

    public override IObservable<Unit> Invalidated => System.Reactive.Linq.Observable.Select(_currentUser.Changes, _ => Unit.Default);

    public RoleRestrictedMenuAccessPolicy(IUserSession currentUser)
    {
        _currentUser = currentUser;
    }

    protected override bool CanAccess(IRoleRestricted item) =>
        item.RequiredRoleIds.All(_currentUser.HasRole);
}
