using ReactiveUI;

namespace MiraiSpace.Presentation.Menu.Demo;

public sealed class RoleToggleMenuItem : MenuItemViewModel, IDisposable
{
    private readonly CurrentUserContext _currentUser;
    private readonly IDisposable _subscription;

    public RoleToggleMenuItem(AppNavigationState navigation, CurrentUserContext currentUser)
        : base(navigation, 900)
    {
        _currentUser = currentUser;
        _subscription = _currentUser.RolesChanged.Subscribe(_ =>
        {
            this.RaisePropertyChanged(nameof(Title));
            this.RaisePropertyChanged(nameof(DisplayTitle));
        });
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

    public void Dispose() => _subscription.Dispose();
}
