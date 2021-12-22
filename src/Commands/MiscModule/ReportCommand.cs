using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using ducker.Logs;

namespace ducker.Commands.MiscModule
{
    public partial class MiscCommands
    {
        [Command("report"), Description("Report mentioned user")]
        public async Task ReportCommand(CommandContext msg, DiscordMember member, [RemainingText] string reason)
        {
            await msg.Message.CreateReactionAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
            await Log.Report(msg.Guild, $"{msg.User.Mention} reported {member.Mention}\nReason: {reason}");
        }
    }
}