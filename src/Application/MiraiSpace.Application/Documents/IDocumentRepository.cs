namespace MiraiSpace.Application.Documents;

public sealed record Document
{
    public Guid Id { get; init; }
    public string Name { get; init; } = "";
    public string Body { get; init; } = "";
    public DateTimeOffset UpdatedAt { get; init; }
}

public sealed record DocumentChanged
{
    public Guid Id { get; }

    public DocumentChanged(Guid id)
    {
        Id = id;
    }
}

public interface IDocumentRepository
{
    ValueTask<IReadOnlyList<Document>> ListAsync(CancellationToken cancellationToken = default);
    ValueTask<Document?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    ValueTask RenameAsync(Guid id, string name, CancellationToken cancellationToken = default);
    ValueTask SaveAsync(Guid id, string body, CancellationToken cancellationToken = default);
    ValueTask DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
