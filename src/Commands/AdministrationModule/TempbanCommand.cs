using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using ducker.Commands.Attributes;
using ducker.Logs;

namespace ducker.Commands.AdministrationModule
{
    public partial class AdministrationCommands
    {
        [Command("tempban"), Description("Temporarily ban mentioned member"), RequireAdmin]
        public async Task TempbanCommand(CommandContext msg, DiscordMember member, int duration,
            string reason = "No reason given")
        {
            await msg.Message.CreateReactionAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
            await member.BanAsync(0, reason);
            await Log.Audit(msg.Guild, $"{msg.User.Mention} banned {member.Mention} for {duration} minutes", reason);
            Thread.Sleep(duration * 60000 * 60);
            await member.UnbanAsync(msg.Guild, reason);
            await Log.Audit(msg.Guild, $"{member.Mention} unbanned", "Ban time expired");
        }
    }
}