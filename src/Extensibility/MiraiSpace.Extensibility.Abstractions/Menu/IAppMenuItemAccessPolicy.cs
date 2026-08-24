namespace MiraiSpace.Extensibility.Abstractions.Menu;

public interface IAppMenuItemAccessPolicy
{
    bool CheckAccess(IAppMenuItem item);

    event EventHandler? AccessChanged;
}
