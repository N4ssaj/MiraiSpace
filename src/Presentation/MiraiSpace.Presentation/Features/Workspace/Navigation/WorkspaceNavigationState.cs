using MiraiSpace.Presentation.Foundation;
using ReactiveUI.SourceGenerators;

namespace MiraiSpace.Presentation.Features.Workspace.Navigation;

public sealed partial class WorkspaceNavigationState : ReactiveModel
{
    [Reactive]
    public partial WorkspacePageViewModel CurrentPage { get; private set; } =
        new(
            "OVERVIEW",
            "Good morning, Alex",
            "Here is what is happening across your workspace today.",
            "#7165E8");

    public void Navigate(
        string eyebrow,
        string title,
        string description,
        string accent)
    {
        CurrentPage = new WorkspacePageViewModel(
            eyebrow,
            title,
            description,
            accent);
    }
}
