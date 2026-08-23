using MiraiSpace.Extensibility.Abstractions.Menu;

namespace MiraiSpace.Presentation.Menu.Demo;

public sealed class AdministrationMenuItem(AppNavigationState navigation)
    : MenuItemViewModel(navigation, "core.administration", 400), IRoleRestricted
{
    public string Title => "Administration";

    public string Subtitle => "Roles & policies";

    public override string DisplayTitle => Title;

    public override string Caption => Subtitle;

    public override string Glyph => "⚙";

    public override string Accent => "#C267E7";

    public IReadOnlyCollection<Guid> RequiredRoleIds { get; } =
        [RoleIds.Administrator];

    public override ValueTask ExecuteAsync(CancellationToken cancellationToken = default)
    {
        Navigation.Navigate(
            "ADMINISTRATION",
            "Access management",
            "Manage roles, permissions, and workspace policies.",
            "#C267E7");
        return ValueTask.CompletedTask;
    }
}
