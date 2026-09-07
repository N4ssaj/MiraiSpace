using System.CommandLine;
using MiraiSpace.Application.Documents;
using MiraiSpace.Presentation.CommandLine;
using MiraiSpace.Presentation.Panels;

namespace MiraiSpace.Presentation.Features.Documents;

public sealed class DocumentCommands : ICommandModule
{
    private readonly IPanelService _panels;
    private readonly IDocumentRepository _documents;

    public DocumentCommands(IPanelService panels, IDocumentRepository documents)
    {
        _panels = panels;
        _documents = documents;
    }

    public IEnumerable<Command> CreateCommands(TextWriter output)
    {
        var documents = new Command("documents", "Open document examples with typed parameters");
        var list = new Command("list", "List example document IDs");
        list.SetAction(async (_, token) =>
        {
            foreach (var item in await _documents.ListAsync(token))
            {
                await output.WriteLineAsync($"{item.Id:D}  {item.Name}");
            }
        });

        var id = new Argument<Guid>("id");
        var workspace = new Option<string>("--workspace") { Description = "Workspace name" };
        var branch = new Option<string>("--branch") { Description = "Workspace branch" };
        var readOnly = new Option<bool>("--read-only") { Description = "Open in preview mode" };
        var section = new Option<string>("--section") { Description = "Document section" };
        var line = new Option<int>("--line") { Description = "Positive cursor line" };
        var tags = new Option<string[]>("--tags") { Description = "Context tags", AllowMultipleArgumentsPerToken = true };
        var open = new Command("open", "Open a document in a new navigation panel");
        open.Arguments.Add(id);
        foreach (var option in new Option[] { workspace, branch, readOnly, section, line, tags })
        {
            open.Options.Add(option);
        }

        open.SetAction(async (result, token) =>
        {
            var parameters = new DocumentOpenParameters
            {
                DocumentId = result.GetValue(id),
                Workspace = new WorkspaceReference
                {
                    Name = result.GetValue(workspace) ?? "personal",
                    Branch = result.GetValue(branch) ?? "main"
                },
                View = new DocumentViewOptions
                {
                    ReadOnly = result.GetValue(readOnly),
                    Section = result.GetValue(section) ?? "notes",
                    CursorLine = result.GetValue(line) == 0 ? 1 : result.GetValue(line)
                },
                Tags = result.GetValue(tags) ?? []
            };
            var panel = await _panels.OpenAsync<DocumentPageViewModel, DocumentOpenParameters>(parameters, token);
            await output.WriteLineAsync(panel.ToString("D"));
        });
        documents.Subcommands.Add(list);
        documents.Subcommands.Add(open);
        return [documents];
    }
}
