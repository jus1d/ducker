using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using ducker.Logs;
using ducker.SlashCommands.Attributes;

namespace ducker.SlashCommands.AdministrationModule;

public partial class AdministrationSlashCommands
{
    [SlashCommand("ban", "Ban mentioned user in current server")]
    [RequireAdmin]
    public async Task BanCommand(InteractionContext msg,
        [Option("user", "User for ban")] DiscordUser user,
        [Option("reason", "Reason for ban this user")]
        string reason = "noneReason")
    {
        await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
        var member = (DiscordMember) user;
        try
        {
            await msg.Guild.BanMemberAsync(member, 0, reason);
            await Log.Audit(msg.Guild, $"{msg.Member.Mention} banned {member.Mention}.", reason);
        }
        catch
        {
            await msg.Channel.SendMessageAsync(new DiscordEmbedBuilder
            {
                Description = "You can't ban this user",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.WarningColor
            });
        }
    }
}