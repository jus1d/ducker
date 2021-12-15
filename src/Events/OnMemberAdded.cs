using DSharpPlus;
using DSharpPlus.EventArgs;

namespace ducker.Events
{
    public partial class EventHandler
    {
        public static async Task OnMemberAdded(DiscordClient client, GuildMemberAddEventArgs e)
        {
            var channel = e.Guild.GetChannel(Database.GetLogsChannel(e.Guild.Id));
            await e.Member.GrantRoleAsync(e.Guild.GetRole(Role.ChelRoleId));
            await channel.SendMessageAsync($"{e.Member.Mention}, just landed on the `{e.Guild.Name}`");
            
            // TODO: logs
        }
    }
}