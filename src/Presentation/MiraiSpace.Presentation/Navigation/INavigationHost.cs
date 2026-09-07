using MiraiSpace.Presentation.Foundation;

namespace MiraiSpace.Presentation.Navigation;

/// <summary>The native stack operations required by the presentation layer.</summary>
public interface INavigationHost
{
    ReactivePage? Current { get; }
    int Count { get; }

    Task PushAsync(ReactivePage model, CancellationToken cancellationToken);
    Task PopAsync(CancellationToken cancellationToken);
}
