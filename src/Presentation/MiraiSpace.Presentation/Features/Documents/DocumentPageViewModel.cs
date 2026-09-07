using Microsoft.Extensions.DependencyInjection;
using MiraiSpace.Application.Documents;
using MiraiSpace.Presentation.Dialogs;
using MiraiSpace.Presentation.Foundation;
using MiraiSpace.Presentation.Lifecycle;
using MiraiSpace.Presentation.Navigation;
using ReactiveUI.SourceGenerators;

namespace MiraiSpace.Presentation.Features.Documents;

public sealed partial class DocumentPageViewModel : ReactivePage, IInitializable<DocumentOpenParameters>
{
    private readonly IDocumentRepository _documents;
    private readonly IDialogService _dialogs;
    private readonly IServiceProvider _services;
    private readonly INavigationService _navigation;
    private readonly Initialization<DocumentOpenParameters> _initialization;
    private Guid _documentId;

    public override string Title => Editor.Name;
    public DocumentEditorComponent Editor { get; }
    public DocumentDetailsComponent Details { get; }

    [Reactive]
    public partial bool HasDocument { get; private set; }

    public DocumentPageViewModel(
        IDocumentRepository documents,
        IDialogService dialogs,
        IServiceProvider services,
        INavigationService navigation,
        DocumentEditorComponent editor,
        DocumentDetailsComponent details)
    {
        _documents = documents;
        _dialogs = dialogs;
        _services = services;
        _navigation = navigation;
        Editor = editor;
        Details = details;
        _initialization = new Initialization<DocumentOpenParameters>(InitializeCoreAsync)
            .Then(editor, parameters => parameters)
            .Then(details, parameters => parameters);
    }

    public ValueTask InitializeAsync(
        DocumentOpenParameters parameters,
        CancellationToken cancellationToken = default) =>
        _initialization.RunAsync(parameters, cancellationToken);

    private async ValueTask InitializeCoreAsync(DocumentOpenParameters parameters, CancellationToken token)
    {
        if (parameters.DocumentId == Guid.Empty || parameters.View.CursorLine < 1)
        {
            throw new ArgumentException("A document ID and a positive cursor line are required.", nameof(parameters));
        }

        _documentId = parameters.DocumentId;
        HasDocument = await _documents.GetAsync(_documentId, token) is not null;
    }

    [ReactiveCommand]
    private async Task Delete(CancellationToken token)
    {
        var dialog = _services.GetRequiredService<DeleteDocumentDialog>();
        if (await _dialogs.ShowAsync(this, dialog, _documentId, token) == true)
        {
            await _documents.DeleteAsync(_documentId, token);
            HasDocument = false;
            await _navigation.GoBackAsync(token);
        }
    }

    [ReactiveCommand]
    private Task OpenDocuments(CancellationToken token) => _navigation.NavigateAsync("/documents", token);
}
