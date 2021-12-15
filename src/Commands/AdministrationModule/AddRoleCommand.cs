using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using ducker.Attributes;

namespace ducker.Commands.AdministrationModule
{
    public partial class AdministrationModule
    {
        /// <summary>
        /// Command to grant a role to mentioned user
        /// </summary>
        /// <param name="msg">The context that the command belongs to</param>
        /// <param name="member">Member to add role</param>
        /// <param name="role">Role to add</param>
        [Command("add-role"), 
         Description("Add a role to mentioned user"),
         RequireAdmin]
        public async Task AddRoleCommand(CommandContext msg, DiscordMember member, DiscordRole role)
        {
            await msg.Message.DeleteAsync();
            if (member.Roles.ToArray().Contains(role))
            {
                await msg.Channel.SendMessageAsync(new DiscordEmbedBuilder
                {
                    Description = "This member currently has this role",
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
                await member.GrantRoleAsync(role);
                await msg.Channel.SendMessageAsync(new DiscordEmbedBuilder
                {
                    Description = $"Complete, {role.Name} added to {member.Mention}",
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
                    Description = "You can't add this role",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.IncorrectEmbedColor
                });
            }
        }
        
        /// <summary>
        /// Overload to send incorrect command embed
        /// </summary>
        /// <param name="msg">The context that command belongs to</param>
        /// <param name="text">Some text</param>
        [Command("add-role")]
        public async Task AddRoleCommand(CommandContext msg, [RemainingText] string text)
        {
            await msg.Channel.SendMessageAsync(new DiscordEmbedBuilder
            {
                Title = "Missing argument",
                Description = "**Usage:** `-add-role <member> <role>`",
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