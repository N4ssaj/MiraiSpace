namespace MiraiSpace.Extensibility.Abstractions.Menu;

public sealed record AppMenuItemDescriptor(
    string Id,
    string? ParentId,
    int Order,
    string Title,
    string Caption = "",
    string Glyph = "•",
    string Accent = "#7165E8",
    string Badge = "")
{
    public AppMenuItemDescriptor Validate()
    {
        if (string.IsNullOrWhiteSpace(Id))
        {
            throw new InvalidOperationException("A menu contribution id is required.");
        }

        if (Id == ParentId)
        {
            throw new InvalidOperationException($"Menu contribution '{Id}' cannot be its own parent.");
        }

        if (string.IsNullOrWhiteSpace(Title))
        {
            throw new InvalidOperationException($"Menu contribution '{Id}' requires a title.");
        }

        return this;
    }
}
