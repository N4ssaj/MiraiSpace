namespace MiraiSpace.Extensibility.Abstractions.Menu;

public interface IRoleRestricted
{
    IReadOnlyCollection<Guid> RequiredRoleIds { get; }
}
