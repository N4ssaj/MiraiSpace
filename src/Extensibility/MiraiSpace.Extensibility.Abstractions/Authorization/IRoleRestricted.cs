namespace MiraiSpace.Extensibility.Abstractions.Authorization;

public interface IRoleRestricted
{
    IReadOnlyList<Guid> RequiredRoleIds { get; }
}
