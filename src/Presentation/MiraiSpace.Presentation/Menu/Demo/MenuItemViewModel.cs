using MiraiSpace.Presentation.Menu.Items;

namespace MiraiSpace.Presentation.Menu.Demo;

public abstract class MenuItemViewModel(
    AppNavigationState navigation,
    int order) : StandardAppMenuItem(order)
{
    protected AppNavigationState Navigation { get; } = navigation;
}
