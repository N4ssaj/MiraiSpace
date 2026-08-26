using MiraiSpace.Presentation.Abstractions.Menu;

namespace MiraiSpace.Presentation.Menu.Composition;

public sealed class AppMenuItemComparer : IAppMenuItemComparer
{
    public int Compare(IAppMenuItem? x, IAppMenuItem? y)
    {
        if (ReferenceEquals(x, y)) return 0;
        if (x is null) return -1;
        if (y is null) return 1;

        bool xOrdered = x is IOrderedAppMenuItem;
        bool yOrdered = y is IOrderedAppMenuItem;
        if (xOrdered && yOrdered)
        {
            return ((IOrderedAppMenuItem)x).Order.CompareTo(((IOrderedAppMenuItem)y).Order);
        }

        if (xOrdered) return -1;
        if (yOrdered) return 1;
        return 0;
    }
}
