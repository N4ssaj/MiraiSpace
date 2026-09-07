namespace MiraiSpace.Presentation.Lifecycle;

public interface IInitializable
{
    ValueTask InitializeAsync(CancellationToken cancellationToken = default);
}

public interface IInitializable<in TParameters>
{
    ValueTask InitializeAsync(TParameters parameters, CancellationToken cancellationToken = default);
}
