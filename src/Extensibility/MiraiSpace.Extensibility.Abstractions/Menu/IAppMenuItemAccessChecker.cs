namespace MiraiSpace.Extensibility.Abstractions.Menu;

public interface IAppMenuItemAccessChecker
{
    bool CheckAccess(IAppMenuItem item);

    event EventHandler? AccessChanged;
}
