using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using ducker.Attributes;

namespace ducker.Commands.MusicModule
{
    public partial class MusicCommands
    {
        [Command("pause"),
         Description("Pause now playing music"),
         RequireMusicChannel]
        public async Task PauseCommand(CommandContext msg)
        {
            if (msg.Member.VoiceState == null || msg.Member.VoiceState.Channel == null)
            {
                await Embed.NotInVoiceChannel(msg).SendAsync(msg.Channel);
                return;
            }
            var lava = msg.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            var connection = node.GetGuildConnection(msg.Member.VoiceState.Guild);
            
            if (connection == null)
            {
                await Embed.NoConnection(msg).SendAsync(msg.Channel);
                return;
            }
            if (connection.CurrentState.CurrentTrack == null)
            {
                await Embed.NoTracksPlaying(msg).SendAsync(msg.Channel);
                return;
            }
            await connection.PauseAsync();
            await msg.Message.CreateReactionAsync(DiscordEmoji.FromName(msg.Client, ":success:"));
        }

        [Command("pause")]
        public async Task Pause(CommandContext msg, params string[] txt)
        {
            await Embed.IncorrectCommand(msg, "-pause").SendAsync(msg.Channel);
        }
    }
}