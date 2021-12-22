using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using ducker.Commands.Attributes;
using ducker.Logs;

namespace ducker.Commands.AdministrationModule
{
    public partial class AdministrationCommands
    {
        [Command("remove-role"), 
         Description("Remove role from mentioned user"),
         RequireAdmin]
        public async Task RemoveRoleCommand(CommandContext msg, DiscordMember member, DiscordRole role, [RemainingText] string reason)
        {
            await msg.Message.DeleteAsync();
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
                    Color = Bot.WarningColor
                });
                return;
            }

            try
            {
                await member.RevokeRoleAsync(role);
                await msg.Message.CreateReactionAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
                await Log.Audit(msg.Guild, $"{msg.Member.Mention} remove role {role.Mention} from {member.Mention}.", reason);
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
                    Color = Bot.WarningColor
                });
            }
        }
        
        [Command("remove-role")]
        public async Task RemoveRoleCommand(CommandContext msg, [RemainingText] string text)
        {
            await msg.Channel.SendMessageAsync(new DiscordEmbedBuilder
            {
                Title = $"Missing argument",
                Description = $"**Usage:** `-remove <member> <role>`",
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