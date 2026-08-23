using System.Collections.ObjectModel;

namespace MiraiSpace.Extensibility.Abstractions.Menu;

public interface IAppMenuItemContainer : IAppMenuItem
{
    ReadOnlyObservableCollection<IAppMenuItem> Items { get; }
}
