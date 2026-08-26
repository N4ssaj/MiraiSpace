using MiraiSpace.Presentation.Abstractions.Menu;

namespace MiraiSpace.Presentation.Menu.Composition;

public sealed class AppMenuItemComparer : IComparer<IAppMenuItem>
{
    public int Compare(IAppMenuItem? x, IAppMenuItem? y)
    {
        if (ReferenceEquals(x, y))
        {
            return 0;
        }

        if (x is null)
        {
            return -1;
        }

        if (y is null)
        {
            return 1;
        }

        return (x, y) switch
        {
            (IOrderedAppMenuItem left, IOrderedAppMenuItem right) =>
                left.Order.CompareTo(right.Order),
            (IOrderedAppMenuItem, _) => -1,
            (_, IOrderedAppMenuItem) => 1,
            _ => 0
        };
    }
}
