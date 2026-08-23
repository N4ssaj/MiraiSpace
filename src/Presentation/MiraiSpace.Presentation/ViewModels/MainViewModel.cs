using System.Collections.ObjectModel;
using MiraiSpace.Extensibility.Abstractions.Menu;
using MiraiSpace.Presentation.Menu;
using MiraiSpace.Presentation.Menu.Demo;

namespace MiraiSpace.Presentation.ViewModels;

public sealed class MainViewModel(
    IAppMenuViewModel menu,
    AppNavigationState navigation) : ViewModelBase
{
    public IAppMenuViewModel Menu { get; } = menu;

    public ReadOnlyObservableCollection<IAppMenuItem> MenuItems => Menu.Items;

    public WorkspaceMenuItemContainer Workspace =>
        MenuItems.OfType<WorkspaceMenuItemContainer>().Single();

    public AppNavigationState Navigation { get; } = navigation;

    public ValueTask ExecuteAsync(IAppMenuItem item) => Menu.ExecuteAsync(item);
}
