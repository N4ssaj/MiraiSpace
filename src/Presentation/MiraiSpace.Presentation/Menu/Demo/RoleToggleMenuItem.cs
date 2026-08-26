using System.Reactive.Disposables;
using ReactiveUI;

namespace MiraiSpace.Presentation.Menu.Demo;

public sealed class RoleToggleMenuItem : MenuItemViewModel
{
    private readonly CurrentUserContext _currentUser;

    public RoleToggleMenuItem(AppNavigationState navigation, CurrentUserContext currentUser)
        : base(navigation, 900)
    {
        _currentUser = currentUser;
    }

    public string Title => _currentUser.IsAdministrator
        ? "Leave admin mode"
        : "Try admin mode";

    public override string DisplayTitle => Title;

    public override string Caption => "Live role refresh";

    public override string Glyph => "✦";

    protected override ValueTask ExecuteAsync(CancellationToken cancellationToken = default)
    {
        _currentUser.ToggleAdministrator();
        return ValueTask.CompletedTask;
    }

    protected override void OnActivated(CompositeDisposable disposables)
    {
        base.OnActivated(disposables);
        disposables.Add(_currentUser.RolesChanged.Subscribe(_ =>
        {
            this.RaisePropertyChanged(nameof(Title));
            this.RaisePropertyChanged(nameof(DisplayTitle));
        }));
    }
}
