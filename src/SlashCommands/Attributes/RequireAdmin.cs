using DSharpPlus;
using DSharpPlus.SlashCommands;

namespace ducker.SlashCommands.Attributes;

/// <summary>
///     Check whether the command invoker has admin permissions at current server
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class RequireAdmin : SlashCheckBaseAttribute
{
    public override async Task<bool> ExecuteChecksAsync(InteractionContext msg)
    {
        var isAdmin = msg.Member.Permissions.HasPermission(Permissions.Administrator) || msg.Member.IsOwner;

        return isAdmin;
    }
}