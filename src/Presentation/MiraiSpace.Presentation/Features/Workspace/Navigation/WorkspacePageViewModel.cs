using MiraiSpace.Presentation.Foundation;
using MiraiSpace.Presentation.Lifecycle;
using MiraiSpace.Presentation.Navigation;
using ReactiveUI.SourceGenerators;

namespace MiraiSpace.Presentation.Features.Workspace.Navigation;

public enum WorkspaceSection
{
    Overview,
    Inbox,
    Calendar,
    Administration
}

public sealed partial class WorkspacePageViewModel : ReactivePage, IInitializable<WorkspaceSection>
{
    private readonly INavigationService _navigation;
    private WorkspaceSection _section;

    public string Eyebrow => _section.ToString().ToUpperInvariant();
    public override string Title => _section switch
    {
        WorkspaceSection.Overview => "A space for your next idea",
        WorkspaceSection.Inbox => "Your inbox",
        WorkspaceSection.Calendar => "Your team, in sync",
        WorkspaceSection.Administration => "Access management",
        _ => string.Empty
    };
    public string Description => _section switch
    {
        WorkspaceSection.Overview => "Open a document in its own panel and keep your workspace in view.",
        WorkspaceSection.Inbox => "A place for conversations and updates.",
        WorkspaceSection.Calendar => "Make room for the work ahead.",
        WorkspaceSection.Administration => "This section follows your current workspace roles.",
        _ => string.Empty
    };
    public string Accent { get; } = "#7165E8";

    public WorkspacePageViewModel(INavigationService navigation)
    {
        _navigation = navigation;
    }

    public ValueTask InitializeAsync(WorkspaceSection section, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _section = section;
        return ValueTask.CompletedTask;
    }

    [ReactiveCommand]
    private Task OpenPages(CancellationToken token) => _navigation.NavigateAsync("/documents", token);

    [ReactiveCommand]
    private Task OpenInbox(CancellationToken token) => _navigation.NavigateAsync("/inbox", token);

    [ReactiveCommand]
    private Task OpenCalendar(CancellationToken token) => _navigation.NavigateAsync("/workspace/calendar", token);
}
