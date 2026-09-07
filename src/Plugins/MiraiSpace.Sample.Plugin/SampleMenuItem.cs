using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using System.Windows.Input;
using MiraiSpace.Extensibility.Abstractions.Menu;
using MiraiSpace.Presentation.Menu.Standard;
using MiraiSpace.Presentation.Navigation;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace MiraiSpace.Sample.Plugin;

public sealed partial class SampleMenuItem : StandardAppMenuItem, IAppMenuItem
{
    private readonly INavigationService _navigation;

    public override string Title => "Playground";
    public override string Caption => "Sample plugin";
    public override string Glyph => "+";
    public override string Accent => "#D29949";
    ICommand IAppMenuItem.ExecuteCommand => ExecuteCommand;

    public SampleMenuItem(INavigationService navigation)
    {
        _navigation = navigation;
    }

    protected override void OnActivated(CompositeDisposable disposables)
    {
        _navigation.WhenAnyValue(model => model.Address)
            .Subscribe(address => IsSelected = address?.Path == "/sample")
            .DisposeWith(disposables);
    }

    [ReactiveCommand]
    private Task Execute(CancellationToken token) => _navigation.NavigateAsync("/sample", token);
}
