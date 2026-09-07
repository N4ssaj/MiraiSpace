namespace MiraiSpace.Presentation.Lifecycle;

/// <summary>Composes one initialization with dependent children, independently of UI activation.</summary>
public sealed class Initialization<TParameters>
{
    private readonly Lock _gate = new();
    private readonly Func<TParameters, CancellationToken, ValueTask> _initialize;
    private readonly List<Func<TParameters, CancellationToken, ValueTask>> _children = [];
    private Task? _completion;

    public Initialization(Func<TParameters, CancellationToken, ValueTask> initialize)
    {
        _initialize = initialize;
    }

    public Initialization<TParameters> Then<TChildParameters>(
        IInitializable<TChildParameters> child,
        Func<TParameters, TChildParameters> parameters)
    {
        AddStep((input, token) => child.InitializeAsync(parameters(input), token));
        return this;
    }

    public Initialization<TParameters> Then(IInitializable child)
    {
        AddStep((_, token) => child.InitializeAsync(token));
        return this;
    }

    public ValueTask RunAsync(TParameters parameters, CancellationToken cancellationToken = default)
    {
        lock (_gate)
        {
            if (_completion is null)
            {
                var completion = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
                _completion = completion.Task;
                _ = CompleteAsync(completion, parameters, cancellationToken);
            }

            return new ValueTask(_completion);
        }
    }

    private void AddStep(Func<TParameters, CancellationToken, ValueTask> step)
    {
        lock (_gate)
        {
            if (_completion is not null)
            {
                throw new InvalidOperationException("Compose initialization before starting it.");
            }

            _children.Add(step);
        }
    }

    private async Task CompleteAsync(
        TaskCompletionSource completion,
        TParameters parameters,
        CancellationToken token)
    {
        try
        {
            await RunCoreAsync(parameters, token);
            completion.TrySetResult();
        }
        catch (OperationCanceledException exception)
        {
            completion.TrySetCanceled(exception.CancellationToken);
        }
        catch (Exception exception)
        {
            completion.TrySetException(exception);
        }
    }

    private async Task RunCoreAsync(TParameters parameters, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        await _initialize(parameters, token);
        foreach (var initialize in _children)
        {
            token.ThrowIfCancellationRequested();
            await initialize(parameters, token);
        }
    }
}
