using System.Reactive;
using System.Reactive.Linq;
using MiraiSpace.Extensibility.Abstractions.Menu;
using MiraiSpace.Presentation.Menu;
using MiraiSpace.Presentation.Menu.Demo;

namespace MiraiSpace.Presentation.Tests.Menu;

public sealed class AppMenuViewModelTests
{
    [Fact]
    public void ContributionsAreFlattenedInParentAndOrderSequence()
    {
        AppMenuViewModel menu = CreateMenu(
            new TestContribution(new("child", "parent", 10, "Child")),
            new TestContribution(new("last", null, 20, "Last")),
            new TestContribution(new("parent", null, 10, "Parent")));

        Assert.Equal(["parent", "child", "last"], menu.Items.Select(item => item.Id));
        Assert.Equal(1, menu.Items[1].Depth);
    }

    [Fact]
    public void MissingParentIsRejected()
    {
        Assert.Throws<InvalidOperationException>(() => CreateMenu(
            new TestContribution(new("child", "missing", 10, "Child"))));
    }

    [Fact]
    public void ContributionCycleIsRejected()
    {
        Assert.Throws<InvalidOperationException>(() => CreateMenu(
            new TestContribution(new("first", "second", 10, "First")),
            new TestContribution(new("second", "first", 20, "Second"))));
    }

    private static AppMenuViewModel CreateMenu(params IAppMenuContribution[] contributions)
    {
        var checker = new AppMenuAccessEvaluator([]);
        return new AppMenuViewModel(
            contributions,
            checker,
            new AppMenuContributionExecutor(checker),
            new AppNavigationState());
    }

    private sealed class TestContribution(AppMenuItemDescriptor descriptor) : IAppMenuContribution
    {
        public AppMenuItemDescriptor Descriptor { get; } = descriptor;
        public IObservable<Unit> Changed => Observable.Never<Unit>();
        public ValueTask ExecuteAsync(CancellationToken cancellationToken = default) => ValueTask.CompletedTask;
    }
}
