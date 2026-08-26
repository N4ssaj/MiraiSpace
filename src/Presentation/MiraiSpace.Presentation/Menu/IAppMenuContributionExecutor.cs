using MiraiSpace.Extensibility.Abstractions.Menu;

namespace MiraiSpace.Presentation.Menu;

public interface IAppMenuContributionExecutor
{
    ValueTask ExecuteAsync(IAppMenuContribution contribution, CancellationToken cancellationToken = default);
}

public sealed class AppMenuContributionExecutor(IAppMenuAccessEvaluator accessEvaluator)
    : IAppMenuContributionExecutor
{
    public ValueTask ExecuteAsync(
        IAppMenuContribution contribution,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(contribution);
        return accessEvaluator.CheckAccess(contribution)
            ? contribution.ExecuteAsync(cancellationToken)
            : ValueTask.CompletedTask;
    }
}
