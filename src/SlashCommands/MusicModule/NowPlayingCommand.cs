using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using DSharpPlus.SlashCommands;
using ducker.SlashCommands.Attributes;

namespace ducker.SlashCommands.MusicModule
{
    public partial class MusicSlashCommands
    {
        [SlashCommand("np", "Display now playing track"), RequireMusicChannel]
        public async Task NowPlayingCommand(InteractionContext msg)
        {
            await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
            if (msg.Member.VoiceState == null || msg.Member.VoiceState.Channel == null)
            {
                await msg.Channel.SendMessageAsync(ducker.Embed.NotInVoiceChannelEmbed(msg));
                return;
            }
            var lava = msg.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            var connection = node.GetGuildConnection(msg.Member.VoiceState.Guild);
            if (connection == null)
            {
                await msg.Channel.SendMessageAsync(ducker.Embed.NoConnectionEmbed(msg));
                return;
            }
            if (connection.CurrentState == null || connection.CurrentState.CurrentTrack == null)
            {
                await msg.Channel.SendMessageAsync(ducker.Embed.NoTracksPlayingEmbed(msg));
                return;
            }

            await msg.Channel.SendMessageAsync(ducker.Embed.NowPlayingEmbed(connection.CurrentState.CurrentTrack,
                msg.User));
        }
    }
}