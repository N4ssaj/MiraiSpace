using DynamicData;
using DynamicData.Binding;
using MiraiSpace.Extensibility.Abstractions.Menu;

namespace MiraiSpace.Presentation.Menu;

public static class AppMenuItemComparers
{
    public static IComparer<IAppMenuItem> Default { get; } =
        SortExpressionComparer<IAppMenuItem>
            .Ascending(x => x.Order)
            .ThenByAscending(x => x.Id);
}
