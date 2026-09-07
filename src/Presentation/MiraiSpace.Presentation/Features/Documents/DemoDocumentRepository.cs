using MessagePipe;
using MiraiSpace.Application.Documents;

namespace MiraiSpace.Presentation.Features.Documents;

public sealed class DemoDocumentRepository : IDocumentRepository
{
    private readonly Lock _gate = new();
    private readonly IPublisher<DocumentChanged> _changes;
    private readonly Dictionary<Guid, Document> _documents;

    public DemoDocumentRepository(IPublisher<DocumentChanged> changes)
    {
        _changes = changes;
        _documents = new[]
        {
            new Document
            {
                Id = Guid.Parse("a1729b15-790e-46c4-8f51-b512543e2261"),
                Name = "A place for good ideas",
                Body = "Collect the ideas you want to return to.\n\nStart with a question, add a few notes, and give the next step a name.",
                UpdatedAt = DateTimeOffset.UtcNow.AddHours(-2)
            },
            new Document
            {
                Id = Guid.Parse("b2946f30-639a-4329-9394-0b76e490a802"),
                Name = "This week's focus",
                Body = "Make the important work visible.\n\n• Sketch the direction\n• Try one small thing\n• Keep what works",
                UpdatedAt = DateTimeOffset.UtcNow.AddDays(-1)
            },
            new Document
            {
                Id = Guid.Parse("c3854800-9775-42ae-8e54-a79df4031e73"),
                Name = "Reading room",
                Body = "A quiet place for links, observations, and passages worth remembering.",
                UpdatedAt = DateTimeOffset.UtcNow.AddDays(-3)
            }
        }.ToDictionary(document => document.Id);
    }

    public ValueTask<IReadOnlyList<Document>> ListAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (_gate)
        {
            return ValueTask.FromResult<IReadOnlyList<Document>>(
                _documents.Values.OrderBy(document => document.Name).ToArray());
        }
    }

    public async ValueTask<Document?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await Task.Delay(80, cancellationToken);
        lock (_gate)
        {
            return _documents.GetValueOrDefault(id);
        }
    }

    public ValueTask RenameAsync(Guid id, string name, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        return UpdateAsync(id, document => document with { Name = name.Trim() }, cancellationToken);
    }

    public ValueTask SaveAsync(Guid id, string body, CancellationToken cancellationToken = default) =>
        UpdateAsync(id, document => document with { Body = body }, cancellationToken);

    public ValueTask DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        bool removed;
        lock (_gate)
        {
            removed = _documents.Remove(id);
        }

        if (removed)
        {
            _changes.Publish(new DocumentChanged(id));
        }

        return ValueTask.CompletedTask;
    }

    private ValueTask UpdateAsync(Guid id, Func<Document, Document> update, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (_gate)
        {
            if (!_documents.TryGetValue(id, out var document))
            {
                throw new InvalidOperationException("This document is no longer available.");
            }

            _documents[id] = update(document) with { UpdatedAt = DateTimeOffset.UtcNow };
        }

        _changes.Publish(new DocumentChanged(id));
        return ValueTask.CompletedTask;
    }
}
