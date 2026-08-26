using MiraiSpace.Presentation.ViewModels;
using ReactiveUI;

namespace MiraiSpace.Presentation.Menu.Demo;

public sealed class AppNavigationState : ModelBase
{
    private string _route = "overview";
    private string _eyebrow = "OVERVIEW";
    private string _title = "Good morning, Alex";
    private string _description = "Here is what is happening across your workspace today.";
    private string _accent = "#7165E8";

    public string Route
    {
        get => _route;
        private set => this.RaiseAndSetIfChanged(ref _route, value);
    }

    public string Eyebrow
    {
        get => _eyebrow;
        private set => this.RaiseAndSetIfChanged(ref _eyebrow, value);
    }

    public string Title
    {
        get => _title;
        private set => this.RaiseAndSetIfChanged(ref _title, value);
    }

    public string Description
    {
        get => _description;
        private set => this.RaiseAndSetIfChanged(ref _description, value);
    }

    public string Accent
    {
        get => _accent;
        private set => this.RaiseAndSetIfChanged(ref _accent, value);
    }

    public void Navigate(string route, string eyebrow, string title, string description, string accent)
    {
        Route = route;
        Eyebrow = eyebrow;
        Title = title;
        Description = description;
        Accent = accent;
    }
}
