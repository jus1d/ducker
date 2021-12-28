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
        [Command("skip"),
         Description("Skip to the next track in queue"),
         RequireMusicChannel]
        public async Task SkipCommand(CommandContext msg)
        {
            try
            {
                LavalinkTrack lavalinkTrack = Bot.Queue[0]; // try use list's element to catch exception
            }
            catch
            {
                await msg.Channel.SendMessageAsync(Embed.ClearQueueEmbed(msg.User));
                return;
            }
            
            var lava = msg.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            var connection = node.GetGuildConnection(msg.Member.VoiceState.Guild);
            await connection.StopAsync();
            await msg.Channel.SendMessageAsync(DiscordEmoji.FromName(msg.Client, ":success:"));
        }
        
        [Command("skip")]
        public async Task SkipCommand(CommandContext msg, params string[] txt)
        {
            await Embed.IncorrectCommand(msg, "-skip").SendAsync(msg.Channel);
        }
    }
}