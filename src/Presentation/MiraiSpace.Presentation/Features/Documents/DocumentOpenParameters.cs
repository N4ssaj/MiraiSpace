namespace MiraiSpace.Presentation.Features.Documents;

public sealed record WorkspaceReference
{
    public string Name { get; init; } = "personal";
    public string Branch { get; init; } = "main";
}

public sealed record DocumentViewOptions
{
    public bool ReadOnly { get; init; }
    public string Section { get; init; } = "notes";
    public int CursorLine { get; init; } = 1;
}

public sealed record DocumentOpenParameters
{
    public required Guid DocumentId { get; init; }
    public WorkspaceReference Workspace { get; init; } = new();
    public DocumentViewOptions View { get; init; } = new();
    public IReadOnlyList<string> Tags { get; init; } = [];
}
