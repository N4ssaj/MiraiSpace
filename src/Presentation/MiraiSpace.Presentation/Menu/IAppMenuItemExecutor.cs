using MiraiSpace.Extensibility.Abstractions.Menu;

namespace MiraiSpace.Presentation.Menu;

public interface IAppMenuItemExecutor
{
    ValueTask ExecuteAsync(
        IAppMenuItem item,
        CancellationToken cancellationToken = default);
}

public sealed class AppMenuItemExecutor(IAppMenuItemAccessChecker accessChecker)
    : IAppMenuItemExecutor
{
    public ValueTask ExecuteAsync(
        IAppMenuItem item,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(item);
        return accessChecker.CheckAccess(item)
            ? item.ExecuteAsync(cancellationToken)
            : ValueTask.CompletedTask;
    }
}
