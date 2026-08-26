namespace MiraiSpace.Presentation.Abstractions.Menu;

public interface IAppMenuItemContainer : IAppMenuItem
{
    IReadOnlyList<IAppMenuItem> Items { get; }
}
