using DSharpPlus.Lavalink;
using DSharpPlus.Lavalink.EventArgs;

namespace ducker.Events
{
    public partial class EventHandler
    {
        public static async Task OnPlaybackFinished(LavalinkGuildConnection sender, TrackFinishEventArgs e)
        {
            try
            {
                LavalinkTrack lavalinkTrack = Bot.Queue[0]; // try use list's element to catch exception
            }
            catch (Exception exception)
            {
                return;
            }

            ulong musicChannelIdFromDb = Database.GetMusicChannel(sender.Guild.Id);

            await sender.PlayAsync(Bot.Queue[0]);
            await Embed.NowPlaying(sender.Node.Discord, Bot.Queue[0], await sender.Node.Discord.GetGuildAsync(696496218934608004).Result.GetMemberAsync(Bot.Id)).SendAsync(sender.Guild.GetChannel(musicChannelIdFromDb));
            Bot.Queue.Remove(Bot.Queue[0]);
        }
    }
}