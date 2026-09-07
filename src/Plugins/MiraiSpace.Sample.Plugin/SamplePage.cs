using MiraiSpace.Presentation.Foundation;
using MiraiSpace.Presentation.Navigation;
using ReactiveUI.SourceGenerators;

namespace MiraiSpace.Sample.Plugin;

public sealed partial class SamplePage : ReactivePage
{
    private readonly INavigationService _navigation;

    public override string Title => "Plugin playground";

    public SamplePage(INavigationService navigation)
    {
        _navigation = navigation;
    }

    [ReactiveCommand]
    private Task OpenDocuments(CancellationToken token) => _navigation.NavigateAsync("/documents", token);
}
