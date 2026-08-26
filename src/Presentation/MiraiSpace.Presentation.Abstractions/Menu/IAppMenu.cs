namespace MiraiSpace.Presentation.Abstractions.Menu;

public interface IAppMenu
{
    IReadOnlyList<IAppMenuItem> Items { get; }
}
