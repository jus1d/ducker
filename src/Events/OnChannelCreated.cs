using DSharpPlus;
using DSharpPlus.EventArgs;
using ducker.Database;

namespace ducker.Events;

public partial class EventHandler
{
    public static async Task OnChannelCreated(DiscordClient client, ChannelCreateEventArgs e)
    {
        if (e.Channel.Type != ChannelType.Text)
            return;

        var muteRole = e.Guild.GetRole(DB.GetId(e.Guild.Id, "muteRoleId"));
        await e.Channel.AddOverwriteAsync(muteRole, Permissions.None, Permissions.SendMessages);

        foreach (var channel in await e.Guild.GetChannelsAsync())
            if (channel.Type == ChannelType.Text)
                await channel.AddOverwriteAsync(muteRole, Permissions.None, Permissions.SendMessages);
    }
}