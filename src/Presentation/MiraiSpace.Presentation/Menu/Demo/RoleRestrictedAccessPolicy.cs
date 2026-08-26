using System.Reactive;
using MiraiSpace.Extensibility.Abstractions.Common;
using MiraiSpace.Presentation.Abstractions.Menu;
using MiraiSpace.Presentation.Menu.Access;

namespace MiraiSpace.Presentation.Menu.Demo;

public sealed class RoleRestrictedAccessPolicy(CurrentUserContext currentUser) : IAppMenuAccessPolicy
{
    public bool CheckAccess(IAppMenuItem item) =>
        item is not IRoleRestricted restricted || restricted.RequiredRoleIds.All(currentUser.HasRole);

    public IObservable<Unit> AccessChanged => currentUser.RolesChanged;
}
