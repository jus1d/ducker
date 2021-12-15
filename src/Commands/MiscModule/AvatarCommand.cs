using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace ducker.Commands.MiscModule
{
    public partial class MiscCommands : BaseCommandModule
    {
        /// <summary>
        /// Command to send embed with user's avatar, and it's URL
        /// </summary>
        /// <param name="msg">The context that command belongs to</param>
        /// <param name="member">Discord member, whose avatar user need</param>
        [Command("avatar"), 
         Description("Send user's avatar and it's link to current channel"), 
         Aliases("ava")]
        public async Task Avatar(CommandContext msg, DiscordMember member)
        {
            await msg.Channel.SendMessageAsync(new DiscordEmbedBuilder
            {
                Title = "User's avatar",
                Description = $"**{member.Mention}'s avatar**",
                ImageUrl = member.AvatarUrl,
                Url = member.AvatarUrl,
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.MainEmbedColor
            });
        }
        
        /// <summary>
        /// Overload to send incorrect command embed
        /// </summary>
        /// <param name="msg">The context that command belongs to</param>
        /// <param name="text">Some text</param>
        [Command("avatar")]
        public async Task Avatar(CommandContext msg, [RemainingText] string text)
        {
            await msg.Channel.SendMessageAsync(new DiscordEmbedBuilder
            {
                Title = "Missing argument",
                Description = "**Usage:** `-avatar <user>`",
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