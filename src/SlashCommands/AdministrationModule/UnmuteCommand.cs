using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using ducker.Database;
using ducker.Logs;
using ducker.SlashCommands.Attributes;

namespace ducker.SlashCommands.AdministrationModule;

public partial class AdministrationSlashCommands
{
    [SlashCommand("unmute", "Unmute mentioned member")]
    [RequireAdmin]
    public async Task UnmuteCommand(InteractionContext msg,
        [Option("member", "Member to unmute")] DiscordUser user,
        [Option("reason", "Reason for unmute")]
        string reason = "noneReason")
    {
        await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
        var member = (DiscordMember) user;
        var muteRoleId = DB.GetId(msg.Guild.Id, "muteRoleId");
        if (muteRoleId == 0)
        {
            await msg.Channel.SendMessageAsync(new DiscordEmbedBuilder
            {
                Description = "Mute role is not configured for this server\nUse `mute` to configure it",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.WarningColor
            });
        }
        else
        {
            await member.RevokeRoleAsync(msg.Guild.GetRole(muteRoleId));
            await Log.Audit(msg.Guild, $"{msg.Member.Mention} unmuted {member.Mention}.", reason);
        }
    }
}