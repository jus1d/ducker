using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using ducker.SlashCommands.Attributes;
using ducker.Logs;

namespace ducker.SlashCommands.AdministrationModule
{
    public partial class AdministrationSlashCommands
    {
        [SlashCommand("tempban", "Temporarily ban mentioned member"), RequireAdmin]
        public async Task TempbanCommand(InteractionContext msg, 
            [Option("member", "Member to ban")] DiscordUser user, 
            [Option("duration", "Ban duration in hours")] long duration,
            [Option("reason", "Reason for ban")] string reason = "noneReason")
        {
            await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
            DiscordMember member = (DiscordMember)user;
            await member.BanAsync(0, reason);
            await Log.Audit(msg.Guild, $"{msg.User.Mention} banned {user.Mention} for {duration} minutes", reason);
            Thread.Sleep((int)duration * 60000 * 60);
            await member.UnbanAsync(msg.Guild, reason);
            await Log.Audit(msg.Guild, $"{user.Mention} unbanned", "Ban time expired");
        }
    }
}