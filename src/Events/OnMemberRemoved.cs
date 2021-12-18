using DSharpPlus;
using DSharpPlus.EventArgs;
using ducker.Logs;

namespace ducker.Events
{
    public partial class EventHandler
    {
        public static async Task OnMemberRemoved(DiscordClient client, GuildMemberRemoveEventArgs e)
        {
            await Log.LogToAudit(e.Guild, $"{e.Member.Mention} just leaved from server");
        }
    }
}