using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using ducker.Attributes;

namespace ducker.Commands.MusicModule
{
    public partial class MusicCommands
    {
        [Command("queue"),
         Description("Send queue list"),
         RequireMusicChannel]
        public async Task QueueCommand(CommandContext msg)
        {
            await msg.Channel.SendMessageAsync(Embed.Queue(msg.User));
        }

        [Command("queue")]
        public async Task QueueCommand(CommandContext msg, [RemainingText] string text)
        {
            await msg.Channel.SendMessageAsync(Embed.Queue(msg.User));
        }
    }
}