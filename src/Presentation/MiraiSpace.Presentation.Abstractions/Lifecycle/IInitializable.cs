namespace MiraiSpace.Presentation.Abstractions.Lifecycle;

public interface IInitializable
{
    ValueTask InitializeAsync(CancellationToken cancellationToken = default);
}

public interface IInitializable<in TParameter>
{
    ValueTask InitializeAsync(
        TParameter parameter,
        CancellationToken cancellationToken = default);
}
