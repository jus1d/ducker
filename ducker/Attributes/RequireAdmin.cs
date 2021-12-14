using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace ducker.Attributes
{
    /// <summary>
    /// Check whether the command invoker has admin permissions at current server
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class RequireAdmin : CheckBaseAttribute
    {
        public override Task<bool> ExecuteCheckAsync(CommandContext msg, bool help)
        {
            bool isAdmin = msg.Member.Permissions.HasPermission(Permissions.Administrator) || msg.Member.IsOwner;
            
            return Task.FromResult(isAdmin);
        }
    }
}