using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using ducker.Database;
using ducker.SlashCommands.Attributes;

namespace ducker.SlashCommands.MusicModule
{
    public partial class MusicSlashCommands
    {
        [SlashCommand("clear-queue", "Clear queue list"), RequireMusicChannel]
        public async Task ClearQueueCommand(InteractionContext msg)
        {
            await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
            Bot.Queue.Clear();
            await msg.Channel.SendMessageAsync(Embed.Queue(msg.User));
        }
    }
}