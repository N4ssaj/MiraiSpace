using System.CommandLine;
using MiraiSpace.Presentation.Navigation;
using MiraiSpace.Presentation.Panels;

namespace MiraiSpace.Presentation.CommandLine;

public sealed class NavigationCommands : ICommandModule
{
    private readonly INavigationService _navigation;
    private readonly IPanelService _panels;

    public NavigationCommands(INavigationService navigation, IPanelService panels)
    {
        _navigation = navigation;
        _panels = panels;
    }

    public IEnumerable<Command> CreateCommands(TextWriter output)
    {
        var address = new Argument<string>("address") { Description = "Registered page URL" };
        var navigate = new Command("navigate", "Open a page in the current navigation panel");
        navigate.Arguments.Add(address);
        navigate.SetAction(async (result, token) =>
        {
            await _navigation.NavigateAsync(result.GetValue(address)!, token);
            await output.WriteLineAsync($"Opened {_navigation.Current?.Title}");
        });

        var back = new Command("back", "Return to the previous page in the current panel");
        back.SetAction(async (_, token) => await _navigation.GoBackAsync(token));

        var panel = new Command("panel", "Manage independent floating navigation panels");
        var openAddress = new Argument<string>("address") { Description = "Initial page URL" };
        var open = new Command("open", "Open a new panel with its own scope and navigation");
        open.Arguments.Add(openAddress);
        open.SetAction(async (result, token) =>
        {
            var id = await _panels.OpenAsync(result.GetValue(openAddress)!, token);
            await output.WriteLineAsync(id.ToString("D"));
        });

        var panelId = new Argument<Guid>("id") { Description = "ID returned by panel open or panel list" };
        var close = new Command("close", "Close a panel and release its scope");
        close.Arguments.Add(panelId);
        close.SetAction(async (result, _) =>
        {
            await _panels.CloseAsync(result.GetValue(panelId));
            await output.WriteLineAsync("Panel closed.");
        });

        var list = new Command("list", "List open floating panels");
        list.SetAction(async (_, _) =>
        {
            foreach (var item in _panels.Panels)
            {
                await output.WriteLineAsync($"{item.Id:D}  {item.Title}");
            }
        });
        var targetId = new Argument<Guid>("id");
        var targetAddress = new Argument<string>("address");
        var navigatePanel = new Command("navigate", "Navigate an existing panel by ID");
        navigatePanel.Arguments.Add(targetId);
        navigatePanel.Arguments.Add(targetAddress);
        navigatePanel.SetAction(async (result, token) =>
            await _panels.NavigateAsync(result.GetValue(targetId), result.GetValue(targetAddress)!, token));
        var backId = new Argument<Guid>("id");
        var backPanel = new Command("back", "Go back in an existing panel");
        backPanel.Arguments.Add(backId);
        backPanel.SetAction(async (result, token) => await _panels.GoBackAsync(result.GetValue(backId), token));
        panel.Subcommands.Add(navigatePanel);
        panel.Subcommands.Add(backPanel);
        panel.Subcommands.Add(open);
        panel.Subcommands.Add(close);
        panel.Subcommands.Add(list);
        return [navigate, back, panel];
    }
}
