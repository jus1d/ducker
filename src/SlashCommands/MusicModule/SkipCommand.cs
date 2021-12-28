using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using DSharpPlus.SlashCommands;
using ducker.DiscordData;
using ducker.SlashCommands.Attributes;

namespace ducker.SlashCommands.MusicModule
{
    public partial class MusicSlashCommands
    {
        [SlashCommand("skip", "Skip to the next track in queue"), RequireMusicChannel]
        public async Task SkipCommand(InteractionContext msg)
        {
            await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
            if (msg.Member.VoiceState == null || msg.Member.VoiceState.Channel == null)
            {
                await msg.Channel.SendMessageAsync(Embed.NotInVoiceChannelEmbed(msg));
                return;
            }
            
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
            if (connection.CurrentState.CurrentTrack == null)
            {
                await msg.Channel.SendMessageAsync(Embed.NoTracksPlayingEmbed(msg));
                return;
            }
            await connection.StopAsync();
        }
    }
}