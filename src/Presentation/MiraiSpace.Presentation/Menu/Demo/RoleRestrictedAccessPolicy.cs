using System.Reactive;
using MiraiSpace.Extensibility.Abstractions.Common;
using MiraiSpace.Extensibility.Abstractions.Menu;

namespace MiraiSpace.Presentation.Menu.Demo;

public sealed class RoleRestrictedAccessPolicy(CurrentUserContext currentUser)
    : IAppMenuAccessPolicy
{
    public bool CheckAccess(IAppMenuContribution contribution) =>
        contribution is not IRoleRestricted restricted
        || restricted.RequiredRoleIds.All(currentUser.HasRole);

    public IObservable<Unit> AccessChanged => currentUser.RolesChanged;
}
