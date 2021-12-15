using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using ducker.Attributes;

namespace ducker.Commands.MusicModule
{
    public partial class MusicCommands
    {
        [Command("remove-from-queue"),
         Description("Remove track from queue by it's position"),
         RequireMusicChannel]
        public async Task RemoveFromQueue(CommandContext msg, uint position)
        {
            ulong musicChannelIdFromDb = Database.GetMusicChannel(msg.Guild.Id);
            ulong cmdChannelIdFromDb = Database.GetCmdChannel(msg.Guild.Id);
            
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

            try
            {
                Bot.Queue.RemoveAt((int)position - 1);
            }
            catch
            {
                await msg.Channel.SendMessageAsync(Embed.InvalidTrackPositionEmbed(msg.User));
            }

            await msg.Channel.SendMessageAsync(Embed.TrackRemovedFromQueueEmbed(msg.User));
        }

        [Command("remove-from-queue")]
        public async Task RemoveFromQueueCommand(CommandContext msg, [RemainingText] string text)
        {
            ulong musicChannelIdFromDb = Database.GetMusicChannel(msg.Guild.Id);
            ulong cmdChannelIdFromDb = Database.GetCmdChannel(msg.Guild.Id);
            
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
            await msg.Channel.SendMessageAsync(Embed.IncorrectCommand(msg, "-remove-from-queue <position>"));
        }
    }
}