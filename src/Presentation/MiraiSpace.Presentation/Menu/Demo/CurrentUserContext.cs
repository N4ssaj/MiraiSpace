using System.Reactive;
using System.Reactive.Subjects;
using MiraiSpace.Presentation.ViewModels;
using ReactiveUI;

namespace MiraiSpace.Presentation.Menu.Demo;

public sealed class CurrentUserContext : ModelBase, IDisposable
{
    private readonly HashSet<Guid> _roles = [];
    private readonly Subject<Unit> _rolesChanged = new();

    public bool IsAdministrator => _roles.Contains(RoleIds.Administrator);
    public IObservable<Unit> RolesChanged => _rolesChanged;
    public bool HasRole(Guid roleId) => _roles.Contains(roleId);

    public void ToggleAdministrator()
    {
        if (!_roles.Add(RoleIds.Administrator)) _roles.Remove(RoleIds.Administrator);
        this.RaisePropertyChanged(nameof(IsAdministrator));
        _rolesChanged.OnNext(Unit.Default);
    }

    public void Dispose() => _rolesChanged.Dispose();
}
