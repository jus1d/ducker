using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using ducker.SlashCommands.Attributes;
using ducker.Database;
using ducker.Logs;

namespace ducker.SlashCommands.AdministrationModule
{
    public partial class AdministrationSlashCommands
    {
        [SlashCommand("unmute", "Unmute mentioned member"), RequireAdmin]
        public async Task UnmuteCommand(InteractionContext msg, [Option("member", "Member to unmute")] DiscordUser user, [Option("reason", "Reason for unmute")] string reason)
        {
            await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
            DiscordMember member = (DiscordMember) user;
            ulong muteRoleId = DB.GetMuteRoleId(msg.Guild.Id);
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
                await Log.LogToAudit(msg.Guild, $"{msg.Member.Mention} unmuted {member.Mention}. Reason: {reason}");
            }
            
            // TODO: fix bug (if delete mute role command doesn't work)
        }
    }
}