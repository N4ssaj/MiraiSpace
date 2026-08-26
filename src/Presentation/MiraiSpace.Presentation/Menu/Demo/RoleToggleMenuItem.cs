using System.Reactive.Disposables;
using MiraiSpace.Presentation.Menu.Standard;
using ReactiveUI;

namespace MiraiSpace.Presentation.Menu.Demo;

public sealed class RoleToggleMenuItem(CurrentUserContext currentUser) : StandardAppMenuItem(900)
{
    public override string Title => currentUser.IsAdministrator ? "Leave admin mode" : "Try admin mode";
    public override string Caption => "Live role refresh";
    public override string Glyph => "✦";

    protected override Task ExecuteAsync(CancellationToken cancellationToken)
    {
        currentUser.ToggleAdministrator();
        this.RaisePropertyChanged(nameof(Title));
        return Task.CompletedTask;
    }

    protected override void OnActivated(CompositeDisposable disposables)
    {
        base.OnActivated(disposables);
        disposables.Add(currentUser.RolesChanged.Subscribe(_ => this.RaisePropertyChanged(nameof(Title))));
    }
}
