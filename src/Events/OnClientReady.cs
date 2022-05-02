using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace ducker.Events;

public partial class EventHandler
{
    public static Task OnClientReady(DiscordClient client, ReadyEventArgs e)
    {
        Bot.Uptime = DateTime.Now;
        client.UpdateStatusAsync(new DiscordActivity
        {
            ActivityType = ActivityType.Playing,
            Name = "with ducks | -help"
        }, UserStatus.Idle);
        return Task.CompletedTask;
    }
}