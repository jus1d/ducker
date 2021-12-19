using DSharpPlus;
using DSharpPlus.SlashCommands;

namespace ducker.SlashCommands.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RequireAdmin : SlashCheckBaseAttribute
    {
        public override async Task<bool> ExecuteChecksAsync(InteractionContext msg)
        {
            bool isAdmin = msg.Member.Permissions.HasPermission(Permissions.Administrator) || msg.Member.IsOwner;
            
            return isAdmin;
        }
    }
}