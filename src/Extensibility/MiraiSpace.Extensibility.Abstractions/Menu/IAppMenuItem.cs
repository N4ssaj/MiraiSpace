namespace MiraiSpace.Extensibility.Abstractions.Menu;

public interface IAppMenuItem
{
    int Order { get; }

    ValueTask ExecuteAsync(CancellationToken cancellationToken = default);
}
