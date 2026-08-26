using System.Windows.Input;

namespace MiraiSpace.Presentation.Abstractions.Menu;

public interface IAppMenuItem
{
    ICommand Command { get; }
}
