using System.Reactive;
using MiraiSpace.Extensibility.Abstractions.Menu;
using MiraiSpace.Presentation.Menu;

namespace MiraiSpace.Presentation.Tests.Menu;

public sealed class AppMenuItemExecutorTests
{
    [Fact]
    public async Task AccessibleItemIsExecuted()
    {
        var item = new RecordingMenuItem();
        var executor = new AppMenuItemExecutor(new StubAccessChecker(allowed: true));

        await executor.ExecuteAsync(item);

        Assert.Equal(1, item.ExecutionCount);
    }

    [Fact]
    public async Task InaccessibleItemIsNotExecuted()
    {
        var item = new RecordingMenuItem();
        var executor = new AppMenuItemExecutor(new StubAccessChecker(allowed: false));

        await executor.ExecuteAsync(item);

        Assert.Equal(0, item.ExecutionCount);
    }

    private sealed class StubAccessChecker(bool allowed) : IAppMenuItemAccessChecker
    {
        public IObservable<Unit> AccessChanged => System.Reactive.Linq.Observable.Never<Unit>();

        public bool CheckAccess(IAppMenuItem item) => allowed;
    }

    private sealed class RecordingMenuItem : IAppMenuItem
    {
        public string Id => "recording";

        public int Order => 100;

        public int ExecutionCount { get; private set; }

        public ValueTask ExecuteAsync(CancellationToken cancellationToken = default)
        {
            ExecutionCount++;
            return ValueTask.CompletedTask;
        }
    }
}
