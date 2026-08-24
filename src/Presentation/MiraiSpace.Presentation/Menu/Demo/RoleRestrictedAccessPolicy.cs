using System.Reactive;
using MiraiSpace.Extensibility.Abstractions.Common;
using MiraiSpace.Extensibility.Abstractions.Menu;

namespace MiraiSpace.Presentation.Menu.Demo;

public sealed class RoleRestrictedAccessPolicy(CurrentUserContext currentUser)
    : IAppMenuItemAccessPolicy
{
    public bool CheckAccess(IAppMenuItem item) =>
        item is not IRoleRestricted restricted
        || restricted.RequiredRoleIds.All(currentUser.HasRole);

    public IObservable<Unit> AccessChanged => currentUser.RolesChanged;
}
