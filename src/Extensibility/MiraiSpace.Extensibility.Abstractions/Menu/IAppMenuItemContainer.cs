namespace MiraiSpace.Extensibility.Abstractions.Menu;

public interface IAppMenuItemContainer : IAppMenuItem
{
    IReadOnlyList<IAppMenuItem> Items { get; }
}
