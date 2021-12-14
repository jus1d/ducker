using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace ducker.Commands.AdministrationModule
{
    public partial class AdministrationModule
    {
        [Command("remove-role"), 
         Description("Remove role from mentioned user"),
         RequirePermissions(Permissions.ManageRoles)]
        public async Task RemoveRole(CommandContext msg, DiscordMember member, DiscordRole role)
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
                    Color = Bot.IncorrectEmbedColor
                });
                return;
            }

            try
            {
                await member.RevokeRoleAsync(role);
                await msg.Channel.SendMessageAsync(new DiscordEmbedBuilder
                {
                    Description = $"Complete, {role.Name} removed from {member.Mention}",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.MainEmbedColor
                });
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