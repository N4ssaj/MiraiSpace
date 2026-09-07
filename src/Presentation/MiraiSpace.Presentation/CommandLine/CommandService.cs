using System.CommandLine;
using MiraiSpace.Presentation.Foundation;

namespace MiraiSpace.Presentation.CommandLine;

public sealed class CommandService : ICommandService
{
    private readonly ICommandModule[] _modules;
    private readonly IUiDispatcher _dispatcher;

    public CommandService(IEnumerable<ICommandModule> modules, IUiDispatcher dispatcher)
    {
        _modules = modules.ToArray();
        _dispatcher = dispatcher;
    }

    public Task<CommandResult> ExecuteAsync(string command, CancellationToken cancellationToken = default) =>
        _dispatcher.InvokeAsync(async () =>
        {
            cancellationToken.ThrowIfCancellationRequested();
            using var output = new StringWriter();
            using var error = new StringWriter();
            var root = new RootCommand("MiraiSpace application commands");
            foreach (var item in _modules.SelectMany(module => module.CreateCommands(output)))
            {
                if (root.Subcommands.Any(existing => existing.Name == item.Name))
                {
                    throw new InvalidOperationException($"Duplicate command: {item.Name}");
                }

                root.Subcommands.Add(item);
            }

            var configuration = new InvocationConfiguration
            {
                Output = output,
                Error = error,
                EnableDefaultExceptionHandler = false,
                ProcessTerminationTimeout = null
            };
            var result = await root.Parse(string.IsNullOrWhiteSpace(command) ? "--help" : command)
                .InvokeAsync(configuration, cancellationToken);
            return new CommandResult
            {
                ExitCode = result,
                Output = output.ToString(),
                Error = error.ToString()
            };
        });
}
