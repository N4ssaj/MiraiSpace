namespace MiraiSpace.Extensibility.Abstractions.Menu;

public interface IAppMenuItemContainer : IAppMenuItem
{
    IList<IAppMenuItem> Items { get; }
}
