namespace MiraiSpace.Presentation.CommandLine;

public sealed record CommandResult
{
    public int ExitCode { get; init; }
    public string Output { get; init; } = string.Empty;
    public string Error { get; init; } = string.Empty;
}

public interface ICommandService
{
    Task<CommandResult> ExecuteAsync(string command, CancellationToken cancellationToken = default);
}
