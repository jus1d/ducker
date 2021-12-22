using DSharpPlus;
using DSharpPlus.EventArgs;
using ducker.Logs;

namespace ducker.Events
{
    public partial class EventHandler
    {
        public static async Task OnMessageUpdated(DiscordClient client, MessageUpdateEventArgs e)
        {
            try
            {
                await Log.Audit(e.Guild,
                    $"{e.Author.Mention} changed the message: `{e.MessageBefore.Content}` -> `{e.Message.Content}` in {e.Channel.Mention}");
            }
            catch
            {
                await Log.Audit(e.Guild, 
                    $"{e.Author.Mention} changed message with {e.Message.Id} ID. `I can't load message before content (message not cached)` -> `{e.Message.Content}` in {e.Channel.Mention}");
            }
        }
    }
}