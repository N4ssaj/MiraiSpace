using System.Windows.Input;
using MiraiSpace.Presentation.Abstractions.Menu;
using MiraiSpace.Presentation.Menu.Composition;

namespace MiraiSpace.Presentation.Tests.Menu;

public sealed class AppMenuItemComparerTests
{
    [Fact]
    public void OptionalOrderSortsOrderedItemsAndLeavesUnorderedItemsAsFallback()
    {
        var comparer = new AppMenuItemComparer();

        Assert.True(comparer.Compare(new OrderedItem(10), new OrderedItem(20)) < 0);
        Assert.True(comparer.Compare(new OrderedItem(10), new UnorderedItem()) < 0);
        Assert.Equal(0, comparer.Compare(new UnorderedItem(), new UnorderedItem()));
    }

    private sealed class OrderedItem(int order) : IAppMenuItem, IOrderedAppMenuItem
    {
        public int Order { get; } = order;
        public ICommand Command => null!;
    }

    private sealed class UnorderedItem : IAppMenuItem
    {
        public ICommand Command => null!;
    }
}
