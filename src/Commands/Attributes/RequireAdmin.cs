using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace ducker.Commands.Attributes;

/// <summary>
///     Check whether the command invoker has admin permissions at current server
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class RequireAdmin : CheckBaseAttribute
{
    public override async Task<bool> ExecuteCheckAsync(CommandContext msg, bool help)
    {
        var isAdmin = msg.Member.Permissions.HasPermission(Permissions.Administrator) || msg.Member.IsOwner;

        return isAdmin;
    }
}