using System.Windows.Input;
using MiraiSpace.Extensibility.Abstractions.Menu;
using MiraiSpace.Presentation.Navigation;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using ReactiveUI;
using MiraiSpace.Presentation.Menu.Standard;
using ReactiveUI.SourceGenerators;

namespace MiraiSpace.Presentation.Features.Workspace.Menu;

public sealed partial class WorkspaceCalendarMenuItem
    : StandardAppMenuItem, IAppMenuItem
{
    private readonly INavigationService _navigation;

    public override string Title => "Calendar";

    public override string Glyph => "□";

    public override string Accent => "#E7A84B";

    ICommand IAppMenuItem.ExecuteCommand => ExecuteCommand;

    public WorkspaceCalendarMenuItem(INavigationService navigation)
    {
        _navigation = navigation;
    }

    protected override void OnActivated(CompositeDisposable disposables)
    {
        _navigation.WhenAnyValue(x => x.Address)
            .ObserveOn(RxSchedulers.MainThreadScheduler)
            .Subscribe(entry => IsSelected = entry?.Path == "/workspace/calendar")
            .DisposeWith(disposables);
    }

    [ReactiveCommand]
    private Task Execute(CancellationToken token) => _navigation.NavigateAsync("/workspace/calendar", token);
}
