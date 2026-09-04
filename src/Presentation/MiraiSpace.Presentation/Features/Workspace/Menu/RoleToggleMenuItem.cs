using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Windows.Input;
using MiraiSpace.Extensibility.Abstractions.Menu;
using MiraiSpace.Presentation.Features.Workspace.Authorization;
using MiraiSpace.Presentation.Menu.Standard;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace MiraiSpace.Presentation.Features.Workspace.Menu;

public sealed partial class RoleToggleMenuItem : StandardAppMenuItem, IAppMenuItem
{
    private readonly CurrentUserContext _currentUser;

    public override string Title => _currentUser.IsAdministrator
        ? "Leave admin mode"
        : "Try admin mode";

    public override string Caption => "Live role refresh";

    public override string Glyph => "✦";

    ICommand IAppMenuItem.ExecuteCommand => ExecuteCommand;

    public RoleToggleMenuItem(CurrentUserContext currentUser)
    {
        _currentUser = currentUser;
    }

    protected override void OnActivated(CompositeDisposable disposables)
    {
        _currentUser.RolesChanged
            .Subscribe(_ => this.RaisePropertyChanged(nameof(Title)))
            .DisposeWith(disposables);
    }

    [ReactiveCommand]
    private void Execute() => _currentUser.ToggleAdministrator();
}
