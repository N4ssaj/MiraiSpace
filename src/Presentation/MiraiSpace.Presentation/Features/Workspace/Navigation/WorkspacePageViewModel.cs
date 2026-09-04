using MiraiSpace.Presentation.Foundation;

namespace MiraiSpace.Presentation.Features.Workspace.Navigation;

public sealed class WorkspacePageViewModel : ReactivePage
{
    public string Eyebrow { get; }

    public string Title { get; }

    public string Description { get; }

    public string Accent { get; }

    public WorkspacePageViewModel(
        string eyebrow,
        string title,
        string description,
        string accent)
    {
        Eyebrow = eyebrow;
        Title = title;
        Description = description;
        Accent = accent;
    }
}
