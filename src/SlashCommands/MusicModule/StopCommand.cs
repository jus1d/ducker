using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using DSharpPlus.SlashCommands;
using ducker.Database;
using ducker.SlashCommands.Attributes;

namespace ducker.SlashCommands.MusicModule
{
    public partial class MusicSlashCommands
    {
        [SlashCommand("stop", "Stop playing"), RequireMusicChannel]
        public async Task StopCommand(InteractionContext msg)
        {
            await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
            var lava = msg.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            var connection = node.GetGuildConnection(msg.Member.VoiceState.Channel.Guild);

            if (connection == null)
            {
                await msg.Channel.SendMessageAsync(Embed.NoConnectionEmbed(msg));
                return;
            }
            await connection.DisconnectAsync();
        }
    }
}