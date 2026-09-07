using System.Globalization;
using MiraiSpace.Presentation.Navigation;

namespace MiraiSpace.Presentation.Features.Documents;

public sealed class DocumentsRoute : NavigationRoute<DocumentsPageViewModel>
{
    public override string Path => "/documents";
}

public sealed class DocumentRoute : NavigationRoute<DocumentPageViewModel, DocumentOpenParameters>
{
    public override string Path => "/documents/{id}";

    public override bool Matches(NavigationAddress address) =>
        address.Path.StartsWith("/documents/", StringComparison.OrdinalIgnoreCase)
        && address.Path.Split('/').Length == 3;

    protected override DocumentOpenParameters ReadParameters(NavigationAddress address)
    {
        if (!Guid.TryParse(address.Path.Split('/')[2], out var id) || id == Guid.Empty)
        {
            throw new FormatException("The document address requires a valid document ID.");
        }

        var query = address.Query;
        var mode = query.GetValueOrDefault("mode", "edit");
        if (mode is not ("edit" or "preview"))
        {
            throw new FormatException("mode must be edit or preview.");
        }

        if (!int.TryParse(query.GetValueOrDefault("line", "1"), NumberStyles.None, CultureInfo.InvariantCulture, out var line)
            || line < 1)
        {
            throw new FormatException("line must be a positive integer.");
        }

        return new DocumentOpenParameters
        {
            DocumentId = id,
            Workspace = new WorkspaceReference
            {
                Name = query.GetValueOrDefault("workspace", "personal"),
                Branch = query.GetValueOrDefault("branch", "main")
            },
            View = new DocumentViewOptions
            {
                ReadOnly = mode == "preview",
                Section = query.GetValueOrDefault("section", "notes"),
                CursorLine = line
            },
            Tags = query.GetValueOrDefault("tags", string.Empty)
                .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
        };
    }
}
