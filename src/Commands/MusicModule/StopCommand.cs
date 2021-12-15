using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using ducker.Attributes;

namespace ducker.Commands.MusicModule
{
    public partial class MusicCommands
    {
        [Command("stop"), 
         Description("Stop playing"),
         Aliases("s"),
         RequireMusicChannel]
        public async Task StopCommand(CommandContext msg)
        {
            var lava = msg.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            var connection = node.GetGuildConnection(msg.Member.VoiceState.Channel.Guild);

            if (connection == null)
            {
                await Embed.NoConnection(msg).SendAsync(msg.Channel);
                return;
            }
            await connection.DisconnectAsync();
            await msg.Message.CreateReactionAsync(DiscordEmoji.FromName(msg.Client, ":success:"));
        }

        [Command("stop")]
        public async Task StopCommand(CommandContext msg, [RemainingText] string text)
        {
            await Embed.IncorrectCommand(msg, "-stop").SendAsync(msg.Channel);
        }
    }
}