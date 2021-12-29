using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using ducker.Logs;
using ducker.SlashCommands.Attributes;

namespace ducker.SlashCommands.AdministrationModule
{
    public partial class AdministrationSlashCommands
    {
        [SlashCommand("remove-role", "Removes role from mentioned member"),  RequireAdmin]
        public async Task RemoveRoleCommand(InteractionContext msg,
            [Option("member", "Member for remove role")] DiscordUser user,
            [Option("role", "Role to remove it")] DiscordRole role,
            [Option("reason", "Reason to remove role")] string reason = "noneReason")
        {
            await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
            DiscordMember member = (DiscordMember) user;
            
            if (!member.Roles.ToArray().Contains(role))
            {
                await msg.Channel.SendMessageAsync(new DiscordEmbedBuilder
                {
                    Description = "This member doesn't have this role",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.IncorrectEmbedColor
                });
                return;
            }
            try
            {
                await member.RevokeRoleAsync(role);
                await msg.Channel.SendMessageAsync(new DiscordEmbedBuilder
                {
                    Description = $"Complete, {role.Name} removed from {user.Mention}",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.MainEmbedColor
                });
                await Log.Audit(msg.Guild, $"{msg.Member.Mention} removed role {role.Mention} from {member.Mention}.", reason);
            }
            catch
            {
                await msg.Channel.SendMessageAsync(new DiscordEmbedBuilder
                {
                    Description = "You can't remove this role",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.IncorrectEmbedColor
                });
            }
        }
    }
}