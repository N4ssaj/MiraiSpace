using System.Reactive;
using System.Reactive.Linq;
using MiraiSpace.Presentation.Foundation;
using ReactiveUI;

namespace MiraiSpace.Presentation.Features.Workspace.Authorization;

public sealed class CurrentUserContext : ReactiveModel
{
    private readonly HashSet<Guid> _roles = [];

    public bool IsAdministrator => _roles.Contains(WorkspaceRoleIds.Administrator);

    public IObservable<Unit> RolesChanged { get; }

    public CurrentUserContext()
    {
        RolesChanged = this
            .WhenAnyValue(context => context.IsAdministrator)
            .Skip(1)
            .Select(_ => Unit.Default)
            .Publish()
            .RefCount();
    }

    public bool HasRole(Guid roleId) => _roles.Contains(roleId);

    public void ToggleAdministrator()
    {
        if (!_roles.Add(WorkspaceRoleIds.Administrator))
        {
            _roles.Remove(WorkspaceRoleIds.Administrator);
        }

        this.RaisePropertyChanged(nameof(IsAdministrator));
    }
}
