using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using MiraiSpace.Presentation.Dialogs;
using MiraiSpace.Presentation.Foundation;
using MiraiSpace.Presentation.Navigation;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace MiraiSpace.Presentation.Panels;

public sealed partial class NavigationPanelViewModel : ReactiveComponent
{
    private readonly IPanelService _panels;

    public Guid Id { get; } = Guid.NewGuid();
    public INavigationService Navigation { get; }
    public INavigationHost Host { get; }
    public IDialogService Dialogs { get; }

    [Reactive]
    public partial string Address { get; set; } = "/";

    [Reactive]
    public partial string Title { get; private set; } = "MiraiSpace";

    public NavigationPanelViewModel(
        INavigationService navigation,
        INavigationHost host,
        IDialogService dialogs,
        IPanelService panels)
    {
        Navigation = navigation;
        Host = host;
        Dialogs = dialogs;
        _panels = panels;
    }

    protected override void OnActivated(CompositeDisposable disposables)
    {
        Navigation.WhenAnyValue(model => model.Current, model => model.Address)
            .ObserveOn(RxSchedulers.MainThreadScheduler)
            .Subscribe(state =>
            {
                Title = string.IsNullOrWhiteSpace(state.Item1?.Title) ? "MiraiSpace" : state.Item1.Title;
                Address = state.Item2?.Url ?? string.Empty;
            })
            .DisposeWith(disposables);
    }

    [ReactiveCommand]
    private Task Open(CancellationToken token) => Navigation.NavigateAsync(Address, token);

    [ReactiveCommand]
    private Task Back(CancellationToken token) => Navigation.GoBackAsync(token);

    [ReactiveCommand]
    private Task Home(CancellationToken token) => Navigation.NavigateAsync("/", token);

    [ReactiveCommand]
    private Task Close() => _panels.CloseAsync(Id);
}
