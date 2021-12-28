using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using ducker.DiscordData;
using ducker.Logs;

namespace ducker.Commands.MiscModule
{
    public partial class MiscCommands
    {
        [Command("report"), Description("Report mentioned user")]
        public async Task ReportCommand(CommandContext msg, DiscordMember member, [RemainingText] string reason = "No reason given")
        {
            await msg.Message.CreateReactionAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
            await Log.Audit(msg.Guild, $"{msg.User.Mention} reported {member.Mention},", reason, LogType.Report);
        }

        [Command("report"), Description("Report mentioned user")]
        public async Task ReportCommand(CommandContext msg, [RemainingText] string reason = "No reason given")
        {
            if (msg.Message.ReferencedMessage == null)
            {
                await msg.Channel.SendMessageAsync(Embed.IncorrectCommand(msg,
                    "report <user or reply to message> <reason>"));
                return;
            }
            await msg.Message.CreateReactionAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
            await Log.Audit(msg.Guild, $"{msg.User.Mention} reported {msg.Message.ReferencedMessage.Author.Mention},", reason, LogType.Report);
        }
    }
}