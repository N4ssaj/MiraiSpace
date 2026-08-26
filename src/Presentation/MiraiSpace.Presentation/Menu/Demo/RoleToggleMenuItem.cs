using System.Reactive;
using MiraiSpace.Extensibility.Abstractions.Menu;

namespace MiraiSpace.Presentation.Menu.Demo;

public sealed class RoleToggleMenuItem(
    AppNavigationState navigation,
    CurrentUserContext currentUser)
    : DemoMenuContribution(navigation, new(
        "admin-mode", null, 900, "Try admin mode", "Live role refresh", "✦"))
{
    public override AppMenuItemDescriptor Descriptor => base.Descriptor with
    {
        Title = currentUser.IsAdministrator ? "Leave admin mode" : "Try admin mode"
    };

    public override IObservable<Unit> Changed => currentUser.RolesChanged;

    public override ValueTask ExecuteAsync(CancellationToken cancellationToken = default)
    {
        currentUser.ToggleAdministrator();
        return ValueTask.CompletedTask;
    }
}
