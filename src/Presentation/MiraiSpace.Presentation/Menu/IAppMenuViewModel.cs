using System.Collections.ObjectModel;
using MiraiSpace.Extensibility.Abstractions.Menu;

namespace MiraiSpace.Presentation.Menu;

public interface IAppMenuViewModel
{
    ReadOnlyObservableCollection<IAppMenuItem> Items { get; }

    ValueTask ExecuteAsync(
        IAppMenuItem item,
        CancellationToken cancellationToken = default);
}
