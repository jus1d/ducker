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
            await msg.Channel.SendMessageAsync(Embed.IncorrectCommand(msg, "-remove-from-queue <position>"));
        }
    }
}