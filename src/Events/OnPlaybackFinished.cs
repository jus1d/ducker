using DSharpPlus.Lavalink;
using DSharpPlus.Lavalink.EventArgs;
using ducker.Database;
using ducker.DiscordData;

namespace ducker.Events;

public partial class EventHandler
{
    public static async Task OnPlaybackFinished(LavalinkGuildConnection sender, TrackFinishEventArgs e)
    {
        try
        {
            var lavalinkTrack = Bot.Queue[0]; // try use list's element to catch exception
        }
        catch
        {
            return;
        }

        var musicChannelIdFromDb = DB.GetId(sender.Guild.Id, "musicChannelId");

        await sender.PlayAsync(Bot.Queue[0]);
        await Embed.NowPlaying(sender.Node.Discord, Bot.Queue[0],
                await (await sender.Node.Discord.GetGuildAsync(Bot.MainGuildId)).GetMemberAsync(Bot.Id))
            .SendAsync(sender.Guild.GetChannel(musicChannelIdFromDb));
        Bot.Queue.Remove(Bot.Queue[0]);
    }
}