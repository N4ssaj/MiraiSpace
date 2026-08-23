namespace MiraiSpace.Extensibility.Abstractions.Menu;

public interface IAppMenuItem
{
    string Id { get; }

    int Order { get; }

    ValueTask ExecuteAsync(CancellationToken cancellationToken = default);
}
