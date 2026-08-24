using MiraiSpace.Extensibility.Abstractions;
using MiraiSpace.Extensibility.Abstractions.Menu;

namespace MiraiSpace.Presentation.Menu.Demo;

public sealed class RoleRestrictedAccessPolicy : IAppMenuItemAccessPolicy, IDisposable
{
    private readonly CurrentUserContext _currentUser;

    public RoleRestrictedAccessPolicy(CurrentUserContext currentUser)
    {
        _currentUser = currentUser;
        _currentUser.RolesChanged += OnRolesChanged;
    }

    public event EventHandler? AccessChanged;

    public bool CheckAccess(IAppMenuItem item) =>
        item is not IRoleRestricted restricted
        || restricted.RequiredRoleIds.All(_currentUser.HasRole);

    public void Dispose() => _currentUser.RolesChanged -= OnRolesChanged;

    private void OnRolesChanged(object? sender, EventArgs e) =>
        AccessChanged?.Invoke(this, EventArgs.Empty);
}
