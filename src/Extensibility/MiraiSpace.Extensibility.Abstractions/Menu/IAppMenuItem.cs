using System.Windows.Input;

namespace MiraiSpace.Extensibility.Abstractions.Menu;

public interface IAppMenuItem
{
    ICommand ExecuteCommand { get; }
}
