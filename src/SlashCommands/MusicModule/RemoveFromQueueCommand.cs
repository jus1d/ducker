using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using ducker.Database;
using ducker.SlashCommands.Attributes;

namespace ducker.SlashCommands.MusicModule
{
    public partial class MusicSlashCommands
    {
        [SlashCommand("remove-from-queue", "Remove track from queue by his index"), RequireMusicChannel]
        public async Task RemoveFromQueueCommand(InteractionContext msg, [Option("position", "Track's position in queue")] string positionInput)
        {
            await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));

            if (!int.TryParse(positionInput, out int position))
            {
                await msg.Channel.SendMessageAsync(Embed.InvalidTrackPositionEmbed(msg.User));
                return;
            }
            try
            {
                Bot.Queue.RemoveAt(position - 1);
            }
            catch
            {
                await msg.Channel.SendMessageAsync(Embed.InvalidTrackPositionEmbed(msg.User));
                return;
            }

            await msg.Channel.SendMessageAsync(Embed.TrackRemovedFromQueueEmbed(msg.User));
        }
    }
}