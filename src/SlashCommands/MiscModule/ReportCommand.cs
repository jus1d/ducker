using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using ducker.Logs;

namespace ducker.SlashCommands.MiscModule
{
    public partial class MiscSlashCommands
    {
        [SlashCommand("report", "Report mentioned member")]
        public async Task ReportCommand(InteractionContext msg, 
            [Option("member", "Member to report")] DiscordUser user,
            [Option("reason", "Reason to report")] string reason = "No reason given")
        {
            DiscordMember member = (DiscordMember) user;

            await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
            await Log.Audit(msg.Guild, $"{msg.User.Mention} reported {member.Mention},", reason, LogType.Report);
        }
    }
}