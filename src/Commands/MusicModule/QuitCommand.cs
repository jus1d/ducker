using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using ducker.Commands.Attributes;
using ducker.DiscordData;

namespace ducker.Commands.MusicModule
{
    public partial class MusicCommands
    {
        [Command("quit"), 
         Description("Quit any voice channel"),
         Aliases("leave", "q"),
         RequireAdmin,
         RequireMusicChannel]
        public async Task QuitCommand(CommandContext msg, [RemainingText] string text)
        {
            var lava = msg.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            var connection = node.GetGuildConnection(msg.Member.VoiceState.Guild);
            if (connection == null)
            {
                await Embed.NoConnection(msg).SendAsync(msg.Channel);
                return;
            }
            await connection.DisconnectAsync();
            await msg.Message.CreateReactionAsync(DiscordEmoji.FromName(msg.Client, ":success:"));
        }
    }
}