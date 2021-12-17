using DSharpPlus;
using DSharpPlus.EventArgs;
using ducker.Logs;

namespace ducker.Events
{
    public partial class EventHandler
    {
        public static async Task OnMessageUpdated(DiscordClient client, MessageUpdateEventArgs e)
        {
            await Log.LogToAudit(e.Guild,$"{e.Author.Mention} changed the message: `{e.MessageBefore.Content}` -> `{e.Message.Content}` in {e.Channel.Mention}");
        }
    }
}