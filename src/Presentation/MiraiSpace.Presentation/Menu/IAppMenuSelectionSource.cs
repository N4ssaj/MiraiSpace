namespace MiraiSpace.Presentation.Menu;

/// <summary>
/// Supplies the current menu destination without coupling menu composition to navigation implementation.
/// </summary>
public interface IAppMenuSelectionSource
{
    string CurrentItemId { get; }

    IObservable<string> SelectionChanged { get; }
}
