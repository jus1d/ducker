using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using ducker.SlashCommands.Attributes;
using ducker.DiscordData;

namespace ducker.SlashCommands.MusicModule
{
    public partial class MusicSlashCommands
    {
        [SlashCommand("queue", "Send queue list"), RequireMusicChannel]
        public async Task QueueCommand(InteractionContext msg)
        {
            await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
            await msg.Channel.SendMessageAsync(Embed.Queue(msg.User));
        }
    }
}