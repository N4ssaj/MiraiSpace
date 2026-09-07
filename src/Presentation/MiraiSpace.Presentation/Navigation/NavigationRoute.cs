using MiraiSpace.Presentation.Foundation;
using MiraiSpace.Presentation.Lifecycle;

namespace MiraiSpace.Presentation.Navigation;

/// <summary>A URL mapping. Data binding belongs here, while DI only registers the mapping.</summary>
public abstract class NavigationRoute
{
    public abstract string Path { get; }
    public abstract Type PageType { get; }

    public virtual bool Matches(NavigationAddress address) =>
        string.Equals(Path, address.Path, StringComparison.OrdinalIgnoreCase);

    public virtual ValueTask InitializeAsync(
        ReactivePage page,
        NavigationAddress address,
        CancellationToken cancellationToken) =>
        page is IInitializable initializable
            ? initializable.InitializeAsync(cancellationToken)
            : ValueTask.CompletedTask;
}

public abstract class NavigationRoute<TPage> : NavigationRoute where TPage : ReactivePage
{
    public override Type PageType => typeof(TPage);
}

public abstract class NavigationRoute<TPage, TParameters> : NavigationRoute<TPage>
    where TPage : ReactivePage, IInitializable<TParameters>
{
    protected abstract TParameters ReadParameters(NavigationAddress address);

    public sealed override ValueTask InitializeAsync(
        ReactivePage page,
        NavigationAddress address,
        CancellationToken cancellationToken) =>
        ((TPage)page).InitializeAsync(ReadParameters(address), cancellationToken);
}
