using System.Reactive;
using System.Reactive.Linq;
using MiraiSpace.Extensibility.Abstractions.Menu;
using MiraiSpace.Presentation.Menu;

namespace MiraiSpace.Presentation.Tests.Menu;

public sealed class AppMenuContributionExecutorTests
{
    [Fact]
    public async Task AccessibleContributionIsExecuted()
    {
        var contribution = new RecordingContribution();
        var executor = new AppMenuContributionExecutor(new StubAccessChecker(allowed: true));

        await executor.ExecuteAsync(contribution);

        Assert.Equal(1, contribution.ExecutionCount);
    }

    [Fact]
    public async Task InaccessibleContributionIsNotExecuted()
    {
        var contribution = new RecordingContribution();
        var executor = new AppMenuContributionExecutor(new StubAccessChecker(allowed: false));

        await executor.ExecuteAsync(contribution);

        Assert.Equal(0, contribution.ExecutionCount);
    }

    private sealed class StubAccessChecker(bool allowed) : IAppMenuAccessEvaluator
    {
        public IObservable<Unit> AccessChanged => Observable.Never<Unit>();
        public bool CheckAccess(IAppMenuContribution contribution) => allowed;
    }

    private sealed class RecordingContribution : IAppMenuContribution
    {
        public AppMenuItemDescriptor Descriptor { get; } = new("test", null, 0, "Test");
        public IObservable<Unit> Changed => Observable.Never<Unit>();
        public int ExecutionCount { get; private set; }

        public ValueTask ExecuteAsync(CancellationToken cancellationToken = default)
        {
            ExecutionCount++;
            return ValueTask.CompletedTask;
        }
    }
}
