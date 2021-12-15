using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using ducker.Attributes;

namespace ducker.Commands.MusicModule
{
    public partial class MusicCommands
    {
        [Command("clear-queue"),
         Description("Clear queue"),
        RequireMusicChannel]
        public async Task ClearQueueCommand(CommandContext msg)
        {
            Bot.Queue.Clear();
            await msg.Channel.SendMessageAsync(Embed.ClearQueueEmbed(msg.User));
        }

        [Command("clear-queue")]
        public async Task ClearQueueCommand(CommandContext msg, [RemainingText] string text)
        {
            await msg.Channel.SendMessageAsync(Embed.IncorrectCommand(msg, "-clear-queue"));
        }
    }
}