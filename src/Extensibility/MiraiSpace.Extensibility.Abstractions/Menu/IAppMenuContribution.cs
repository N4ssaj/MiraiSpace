using System.Reactive;

namespace MiraiSpace.Extensibility.Abstractions.Menu;

/// <summary>
/// A declarative contribution to the application menu.
/// </summary>
public interface IAppMenuContribution
{
    AppMenuItemDescriptor Descriptor { get; }

    /// <summary>
    /// Announces that the descriptor presentation has changed.
    /// </summary>
    IObservable<Unit> Changed { get; }

    ValueTask ExecuteAsync(CancellationToken cancellationToken = default);
}
