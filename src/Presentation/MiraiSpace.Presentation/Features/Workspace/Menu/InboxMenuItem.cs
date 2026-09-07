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

public sealed partial class InboxMenuItem : StandardAppMenuItem, IAppMenuItem
{
    private readonly INavigationService _navigation;

    public override string Title => "Inbox";

    public override string Glyph => "✉";

    public override string Accent => "#ED6A5A";

    public int UnreadCount { get; } = 8;

    ICommand IAppMenuItem.ExecuteCommand => ExecuteCommand;

    public InboxMenuItem(INavigationService navigation)
    {
        _navigation = navigation;
    }

    protected override void OnActivated(CompositeDisposable disposables)
    {
        _navigation.WhenAnyValue(x => x.Address)
            .ObserveOn(RxSchedulers.MainThreadScheduler)
            .Subscribe(entry => IsSelected = entry?.Path == "/inbox")
            .DisposeWith(disposables);
    }

    [ReactiveCommand]
    private Task Execute(CancellationToken token) => _navigation.NavigateAsync("/inbox", token);
}
