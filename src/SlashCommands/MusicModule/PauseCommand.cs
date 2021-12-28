using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using DSharpPlus.SlashCommands;
using ducker.DiscordData;
using ducker.SlashCommands.Attributes;

namespace ducker.SlashCommands.MusicModule
{
    public partial class MusicSlashCommands
    {
        [SlashCommand("pause", "Pause current track"), RequireMusicChannel]
        public async Task PauseCommand(InteractionContext msg)
        {
            await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
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
            if (connection.CurrentState.CurrentTrack == null)
            {
                await msg.Channel.SendMessageAsync(Embed.NoTracksPlayingEmbed(msg));
                return;
            }
            await connection.PauseAsync();
        }
    }
}