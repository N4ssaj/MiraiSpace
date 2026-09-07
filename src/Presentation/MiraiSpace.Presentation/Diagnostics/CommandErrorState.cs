using System.Reactive.Concurrency;
using Microsoft.Extensions.Logging;
using MiraiSpace.Presentation.Foundation;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace MiraiSpace.Presentation.Diagnostics;

/// <summary>The application's common ReactiveUI command exception observer.</summary>
public sealed partial class CommandErrorState : ReactiveModel, IObserver<Exception>
{
    private readonly ILogger<CommandErrorState> _logger;

    [Reactive]
    public partial string? Message { get; private set; }

    public CommandErrorState(ILogger<CommandErrorState> logger)
    {
        _logger = logger;
    }

    public void Clear() => Message = null;

    public void OnNext(Exception error)
    {
        if (error is OperationCanceledException)
        {
            return;
        }

        _logger.LogError(error, "An application command failed");
        RxSchedulers.MainThreadScheduler.Schedule(() =>
            Message = error is FormatException ? error.Message :
                "The command could not complete. Please try again.");
    }

    public void OnError(Exception error) => OnNext(error);

    public void OnCompleted()
    {
    }
}
