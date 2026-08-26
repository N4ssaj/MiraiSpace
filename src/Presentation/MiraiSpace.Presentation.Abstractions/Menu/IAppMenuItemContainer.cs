namespace MiraiSpace.Presentation.Abstractions.Menu;

public interface IAppMenuItemContainer : IAppMenuItem
{
    bool HasChildren { get; }

    IReadOnlyList<IAppMenuItem> Items { get; }
}
