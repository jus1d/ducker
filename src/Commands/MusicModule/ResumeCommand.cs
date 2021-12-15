using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using ducker.Attributes;

namespace ducker.Commands.MusicModule
{
    public partial class MusicCommands
    {
        [Command("resume"),
         Description("Resume playing music"),
         RequireMusicChannel]
        public async Task ResumeCommand(CommandContext msg)
        {
            if (msg.Member.VoiceState == null || msg.Member.VoiceState.Channel == null)
            {
                await Embed.NotInVoiceChannel(msg).SendAsync(msg.Channel);
                return;
            }
            
            var lava = msg.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            var connection = node.GetGuildConnection(msg.Member.VoiceState.Guild);
            
            if (connection.CurrentState.CurrentTrack == null)
            {
                await Embed.NoTracksPlaying(msg).SendAsync(msg.Channel);
                return;
            }
            await connection.ResumeAsync();
            await msg.Message.CreateReactionAsync(DiscordEmoji.FromName(msg.Client, ":success:"));
        }

        [Command("resume")]
        public async Task ResumeCommand(CommandContext msg, [RemainingText] string text)
        {
            await msg.Channel.SendMessageAsync(Embed.IncorrectCommand(msg, "-resume"));
        }
    }
}