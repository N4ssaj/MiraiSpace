using MiraiSpace.Presentation.CommandLine;
using MiraiSpace.Presentation.Foundation;
using ReactiveUI.SourceGenerators;

namespace MiraiSpace.Presentation.Features.CommandConsole;

public sealed partial class CommandConsoleViewModel : ReactiveComponent
{
    private readonly ICommandService _commands;
    private CancellationTokenSource? _running;

    [Reactive]
    public partial string Input { get; set; } = "--help";

    [Reactive]
    public partial string Output { get; private set; } = "Enter --help to discover application commands.";

    [Reactive]
    public partial bool IsOpen { get; private set; }

    public CommandConsoleViewModel(ICommandService commands)
    {
        _commands = commands;
    }

    [ReactiveCommand]
    private void Toggle() => IsOpen = !IsOpen;

    [ReactiveCommand]
    private async Task Execute(CancellationToken token)
    {
        using var cancellation = CancellationTokenSource.CreateLinkedTokenSource(token);
        _running = cancellation;
        try
        {
            var command = Input;
            var result = await _commands.ExecuteAsync(command, cancellation.Token);
            Output = $"> {command}\n{result.Output}{result.Error}";
        }
        finally
        {
            _running = null;
        }
    }

    [ReactiveCommand]
    private void Cancel() => _running?.Cancel();
}
