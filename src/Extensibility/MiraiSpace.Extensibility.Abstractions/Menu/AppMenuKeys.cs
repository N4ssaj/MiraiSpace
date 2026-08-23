namespace MiraiSpace.Extensibility.Abstractions.Menu;

public static class AppMenuKeys
{
    public const string RootValue = "miraispace.menu.root";

    public const string WorkspaceValue = "miraispace.menu.workspace";

    public static AppMenuKey Root { get; } = new(RootValue);

    public static AppMenuKey Workspace { get; } = new(WorkspaceValue);
}
