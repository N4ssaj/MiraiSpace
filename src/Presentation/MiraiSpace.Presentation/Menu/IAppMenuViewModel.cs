using MiraiSpace.Extensibility.Abstractions.Menu;

namespace MiraiSpace.Presentation.Menu;

public interface IAppMenuViewModel
{
    IReadOnlyList<IAppMenuItem> Items { get; }

    ValueTask ExecuteAsync(
        IAppMenuItem item,
        CancellationToken cancellationToken = default);
}
