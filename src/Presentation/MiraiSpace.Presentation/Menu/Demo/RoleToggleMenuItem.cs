using ReactiveUI;

namespace MiraiSpace.Presentation.Menu.Demo;

public sealed class RoleToggleMenuItem : MenuItemViewModel
{
    private readonly CurrentUserContext _currentUser;

    public RoleToggleMenuItem(AppNavigationState navigation, CurrentUserContext currentUser)
        : base(navigation, "admin-mode", 900)
    {
        _currentUser = currentUser;
        Own(_currentUser.RolesChanged.Subscribe(_ =>
        {
            this.RaisePropertyChanged(nameof(Title));
            this.RaisePropertyChanged(nameof(DisplayTitle));
        }));
    }

    public string Title => _currentUser.IsAdministrator
        ? "Leave admin mode"
        : "Try admin mode";

    public override string DisplayTitle => Title;

    public override string Caption => "Live role refresh";

    public override string Glyph => "✦";

    public override ValueTask ExecuteAsync(CancellationToken cancellationToken = default)
    {
        _currentUser.ToggleAdministrator();
        return ValueTask.CompletedTask;
    }
}
