using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Lavalink;
using ducker.Commands.Attributes;
using ducker.DiscordData;

namespace ducker.Commands.MusicModule
{
    public partial class MusicCommands
    {
        [Command("np"),
         Description("Display now playing track"),
         RequireMusicChannel]
        public async Task NowPlayingCommand(CommandContext msg)
        {
            if (msg.Member.VoiceState == null || msg.Member.VoiceState.Channel == null)
            {
                await msg.Channel.SendMessageAsync(Embed.NotInVoiceChannelEmbed(msg));
                return;
            }
            var lava = msg.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            var connection = node.GetGuildConnection(msg.Member.VoiceState.Guild);
            if (connection == null)
            {
                await msg.Channel.SendMessageAsync(Embed.NoConnectionEmbed(msg));
                return;
            }
            if (connection.CurrentState == null || connection.CurrentState.CurrentTrack == null)
            {
                await msg.Channel.SendMessageAsync(Embed.NoTracksPlayingEmbed(msg));
                return;
            }

            await msg.Channel.SendMessageAsync(Embed.NowPlayingEmbed(connection.CurrentState.CurrentTrack,
                msg.User));
        }

        [Command("np")]
        public async Task NowPlayingCommand(CommandContext msg, [RemainingText] string text)
        {
            await msg.Channel.SendMessageAsync(Embed.IncorrectCommand(msg, "-np"));
        }
    }
}