using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using ducker.Database;

namespace ducker.SlashCommands.MusicModule
{
    public partial class MusicSlashCommands
    {
        [SlashCommand("remove-from-queue", "Remove track from queue by his index")]
        public async Task RemoveFromQueueCommand(InteractionContext msg, [Option("position", "Track's position in queue")] string positionInput)
        {
            await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
            ulong musicChannelIdFromDb = DB.GetMusicChannel(msg.Guild.Id);
            ulong cmdChannelIdFromDb = DB.GetCmdChannel(msg.Guild.Id);
            
            if (musicChannelIdFromDb == 0)
            {
                await msg.Channel.SendMessageAsync(Embed.NoMusicChannelConfigured(msg.User));
                return;
            }
            if (msg.Channel.Id != musicChannelIdFromDb && msg.Channel.Id != cmdChannelIdFromDb)
            {
                await msg.Channel.SendMessageAsync(Embed.IncorrectMusicChannelEmbed(msg, musicChannelIdFromDb));
                return;
            }

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