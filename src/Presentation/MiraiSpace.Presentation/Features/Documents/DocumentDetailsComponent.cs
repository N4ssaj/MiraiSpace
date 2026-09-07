using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Windows.Input;
using MessagePipe;
using MiraiSpace.Application.Documents;
using MiraiSpace.Presentation.Foundation;
using MiraiSpace.Presentation.Lifecycle;
using ReactiveUI.SourceGenerators;

namespace MiraiSpace.Presentation.Features.Documents;

public sealed partial class DocumentDetailsComponent : ReactiveComponent, IInitializable<DocumentOpenParameters>
{
    private readonly IDocumentRepository _documents;
    private readonly ISubscriber<DocumentChanged> _changes;
    private Guid _documentId;

    [Reactive]
    public partial string Name { get; private set; } = string.Empty;

    [Reactive]
    public partial string Updated { get; private set; } = string.Empty;

    [Reactive]
    public partial int WordCount { get; private set; }

    public string ShortId => _documentId.ToString("D")[..8];
    public string Workspace { get; private set; } = string.Empty;
    public string Position { get; private set; } = string.Empty;
    public string Tags { get; private set; } = string.Empty;

    public DocumentDetailsComponent(IDocumentRepository documents, ISubscriber<DocumentChanged> changes)
    {
        _documents = documents;
        _changes = changes;
    }

    public async ValueTask InitializeAsync(DocumentOpenParameters parameters, CancellationToken cancellationToken = default)
    {
        _documentId = parameters.DocumentId;
        Workspace = $"{parameters.Workspace.Name} / {parameters.Workspace.Branch}";
        Position = $"{parameters.View.Section} · line {parameters.View.CursorLine}";
        Tags = string.Join(" · ", parameters.Tags);
        await ReloadAsync(cancellationToken);
    }

    protected override void OnActivated(CompositeDisposable disposables)
    {
        ((ICommand)RefreshCommand).Execute(null);
        _changes.Subscribe(change =>
        {
            if (change.Id == _documentId)
            {
                ((ICommand)RefreshCommand).Execute(null);
            }
        }).DisposeWith(disposables);
    }

    [ReactiveCommand]
    private Task Refresh(CancellationToken token) => ReloadAsync(token).AsTask();

    private async ValueTask ReloadAsync(CancellationToken token)
    {
        var document = await _documents.GetAsync(_documentId, token);
        Name = document?.Name ?? "Document removed";
        Updated = document?.UpdatedAt.ToLocalTime().ToString("dd MMM · HH:mm") ?? string.Empty;
        WordCount = document?.Body.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries).Length ?? 0;
    }
}
