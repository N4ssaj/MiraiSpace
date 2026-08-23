namespace MiraiSpace.Extensibility.Abstractions.Menu;

public readonly record struct AppMenuKey(string Value)
{
    public override string ToString() => Value;
}
