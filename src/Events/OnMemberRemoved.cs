using DSharpPlus;
using DSharpPlus.EventArgs;

namespace ducker.Events
{
    public partial class EventHandler
    {
        public static async Task OnMemberRemoved(DiscordClient client, GuildMemberRemoveEventArgs e)
        {
            var channel = e.Guild.GetChannel(Database.GetLogsChannel(e.Guild.Id));
            await channel.SendMessageAsync($"{e.Member.Mention}. On siebalsya ksta");
            
            // TODO: logs
        }
    }
}