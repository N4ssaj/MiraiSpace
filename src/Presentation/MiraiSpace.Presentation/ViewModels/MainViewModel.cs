using MiraiSpace.Presentation.Features.Workspace.Navigation;
using MiraiSpace.Presentation.Foundation;
using MiraiSpace.Presentation.Menu;

namespace MiraiSpace.Presentation.ViewModels;

public sealed class MainViewModel : ReactivePage
{
    public AppMenuViewModel Menu { get; }

    public WorkspaceNavigationState Navigation { get; }

    public MainViewModel(
        AppMenuViewModel menu,
        WorkspaceNavigationState navigation)
    {
        Menu = menu;
        Navigation = navigation;
    }
}
