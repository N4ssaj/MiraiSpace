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

public sealed partial class OverviewMenuItem : StandardAppMenuItem, IAppMenuItem
{
    private readonly INavigationService _navigation;

    public override string Title => "Overview";

    public override string Caption => "Your daily pulse";

    public override string Glyph => "⌂";

    ICommand IAppMenuItem.ExecuteCommand => ExecuteCommand;

    public OverviewMenuItem(INavigationService navigation)
    {
        _navigation = navigation;
    }

    protected override void OnActivated(CompositeDisposable disposables)
    {
        _navigation.WhenAnyValue(x => x.Address)
            .ObserveOn(RxSchedulers.MainThreadScheduler)
            .Subscribe(entry => IsSelected = entry?.Path == "/")
            .DisposeWith(disposables);
    }

    [ReactiveCommand]
    private Task Execute(CancellationToken token) => _navigation.NavigateAsync("/", token);
}
