using ReactiveUI;

namespace MiraiSpace.Presentation.Menu.Demo;

public sealed class RoleToggleMenuItem : MenuItemViewModel, IDisposable
{
    private readonly CurrentUserContext _currentUser;

    public RoleToggleMenuItem(AppNavigationState navigation, CurrentUserContext currentUser)
        : base(navigation, 900)
    {
        _currentUser = currentUser;
        _currentUser.RolesChanged += OnRolesChanged;
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

    public void Dispose() => _currentUser.RolesChanged -= OnRolesChanged;

    private void OnRolesChanged(object? sender, EventArgs e)
    {
        this.RaisePropertyChanged(nameof(Title));
        this.RaisePropertyChanged(nameof(DisplayTitle));
    }
}
