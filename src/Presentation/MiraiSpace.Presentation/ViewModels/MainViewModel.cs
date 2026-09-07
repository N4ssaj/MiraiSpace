using MiraiSpace.Presentation.Diagnostics;
using MiraiSpace.Presentation.Features.CommandConsole;
using MiraiSpace.Presentation.Foundation;
using MiraiSpace.Presentation.Menu;
using MiraiSpace.Presentation.Panels;

namespace MiraiSpace.Presentation.ViewModels;

public sealed class MainViewModel : ReactivePage
{
    public AppMenuViewModel Menu { get; }
    public NavigationPanelViewModel MainPanel { get; }
    public IPanelService Panels { get; }
    public CommandConsoleViewModel Console { get; }
    public CommandErrorState Errors { get; }

    public MainViewModel(
        AppMenuViewModel menu,
        NavigationPanelViewModel mainPanel,
        IPanelService panels,
        CommandConsoleViewModel console,
        CommandErrorState errors)
    {
        Menu = menu;
        MainPanel = mainPanel;
        Panels = panels;
        Console = console;
        Errors = errors;
    }
}
