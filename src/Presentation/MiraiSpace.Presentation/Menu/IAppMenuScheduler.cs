using System.Reactive.Concurrency;

namespace MiraiSpace.Presentation.Menu;

public interface IAppMenuScheduler
{
    IScheduler Scheduler { get; }
}

public sealed class AppMenuScheduler(IScheduler scheduler) : IAppMenuScheduler
{
    public IScheduler Scheduler { get; } = scheduler;
}
