using System.CommandLine;
using MiraiSpace.Presentation.CommandLine;
using MiraiSpace.Presentation.Navigation;

namespace MiraiSpace.Sample.Plugin;

public sealed class SampleCommands : ICommandModule
{
    private readonly INavigationService _navigation;

    public SampleCommands(INavigationService navigation)
    {
        _navigation = navigation;
    }

    public IEnumerable<Command> CreateCommands(TextWriter output)
    {
        var sample = new Command("sample", "Commands supplied by MiraiSpace.Sample.Plugin");
        var open = new Command("open", "Open the plugin playground");
        open.SetAction(async (_, token) => await _navigation.NavigateAsync<SamplePage>(token));

        var text = new Argument<string>("text") { Description = "Text to return; quote text containing spaces" };
        var echo = new Command("echo", "Return text through the application's command engine");
        echo.Arguments.Add(text);
        echo.SetAction(async (result, token) =>
            await output.WriteLineAsync(result.GetValue(text).AsMemory(), token));
        sample.Subcommands.Add(open);
        sample.Subcommands.Add(echo);
        return [sample];
    }
}
