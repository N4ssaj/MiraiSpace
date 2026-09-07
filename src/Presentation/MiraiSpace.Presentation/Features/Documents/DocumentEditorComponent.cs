using Microsoft.Extensions.DependencyInjection;
using MiraiSpace.Application.Documents;
using MiraiSpace.Presentation.Dialogs;
using MiraiSpace.Presentation.Foundation;
using MiraiSpace.Presentation.Lifecycle;
using ReactiveUI.SourceGenerators;

namespace MiraiSpace.Presentation.Features.Documents;

public sealed partial class DocumentEditorComponent : ReactiveComponent, IInitializable<DocumentOpenParameters>
{
    private readonly IDocumentRepository _documents;
    private readonly IDialogService _dialogs;
    private readonly IServiceProvider _services;
    private Guid _documentId;

    [Reactive]
    public partial string Name { get; private set; } = string.Empty;

    [Reactive]
    public partial string Body { get; set; } = string.Empty;

    [Reactive]
    public partial bool IsReadOnly { get; private set; }

    [Reactive]
    public partial string Status { get; private set; } = "Ready to write";

    public DocumentEditorComponent(
        IDocumentRepository documents,
        IDialogService dialogs,
        IServiceProvider services)
    {
        _documents = documents;
        _dialogs = dialogs;
        _services = services;
    }

    public async ValueTask InitializeAsync(DocumentOpenParameters parameters, CancellationToken cancellationToken = default)
    {
        _documentId = parameters.DocumentId;
        var document = await _documents.GetAsync(_documentId, cancellationToken);
        Name = document?.Name ?? "Document unavailable";
        Body = document?.Body ?? string.Empty;
        IsReadOnly = parameters.View.ReadOnly;
        Status = IsReadOnly ? "Preview · read only" : "Ready to write";
    }

    [ReactiveCommand]
    private async Task Rename(CancellationToken token)
    {
        if (IsReadOnly)
        {
            return;
        }

        var dialog = _services.GetRequiredService<RenameDocumentDialog>();
        if (await _dialogs.ShowAsync(this, dialog, _documentId, token) == true)
        {
            await _documents.RenameAsync(_documentId, dialog.Form.Name.Trim(), token);
            Name = dialog.Form.Name.Trim();
            Status = "Name updated";
        }
    }

    [ReactiveCommand]
    private async Task Save(CancellationToken token)
    {
        if (!IsReadOnly)
        {
            await _documents.SaveAsync(_documentId, Body, token);
            Status = "Saved in this workspace";
        }
    }
}
