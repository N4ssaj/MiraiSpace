using System.CommandLine;

namespace MiraiSpace.Presentation.CommandLine;

/// <summary>A feature or plugin contributes commands using the same parser and execution boundary.</summary>
public interface ICommandModule
{
    IEnumerable<Command> CreateCommands(TextWriter output);
}
