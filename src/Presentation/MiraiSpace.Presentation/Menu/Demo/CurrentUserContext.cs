using ReactiveUI;

namespace MiraiSpace.Presentation.Menu.Demo;

public sealed class CurrentUserContext : ReactiveObject
{
    private readonly HashSet<Guid> _roles = [];

    public bool IsAdministrator => _roles.Contains(RoleIds.Administrator);

    public event EventHandler? RolesChanged;

    public bool HasRole(Guid roleId) => _roles.Contains(roleId);

    public void ToggleAdministrator()
    {
        if (!_roles.Add(RoleIds.Administrator))
        {
            _roles.Remove(RoleIds.Administrator);
        }

        this.RaisePropertyChanged(nameof(IsAdministrator));
        RolesChanged?.Invoke(this, EventArgs.Empty);
    }
}
