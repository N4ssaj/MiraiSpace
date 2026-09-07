using System.Windows.Input;
using MiraiSpace.Extensibility.Abstractions.Authorization;
using MiraiSpace.Extensibility.Abstractions.Menu;
using MiraiSpace.Presentation.Features.Workspace.Authorization;
using MiraiSpace.Presentation.Navigation;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using ReactiveUI;
using MiraiSpace.Presentation.Menu.Standard;
using ReactiveUI.SourceGenerators;

namespace MiraiSpace.Presentation.Features.Workspace.Menu;

public sealed partial class AdministrationMenuItem
    : StandardAppMenuItem, IAppMenuItem, IRoleRestricted
{
    private readonly INavigationService _navigation;

    public override string Title => "Administration";

    public override string Caption => "Roles & policies";

    public override string Glyph => "⚙";

    public override string Accent => "#C267E7";

    public IReadOnlyList<Guid> RequiredRoleIds { get; } =
        [WorkspaceRoleIds.Administrator];

    ICommand IAppMenuItem.ExecuteCommand => ExecuteCommand;

    public AdministrationMenuItem(INavigationService navigation)
    {
        _navigation = navigation;
    }

    protected override void OnActivated(CompositeDisposable disposables)
    {
        _navigation.WhenAnyValue(x => x.Address)
            .ObserveOn(RxSchedulers.MainThreadScheduler)
            .Subscribe(entry => IsSelected = entry?.Path == "/administration")
            .DisposeWith(disposables);
    }

    [ReactiveCommand]
    private Task Execute(CancellationToken token) => _navigation.NavigateAsync("/administration", token);
}
