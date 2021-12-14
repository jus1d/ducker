using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace ducker.Commands.MiscModule
{
    public partial class MiscCommands : BaseCommandModule
    {
        [Command("avatar"), 
         Description("Send user's avatar and it's link to current channel"), 
         Aliases("ava")]
        public async Task Avatar(CommandContext msg, DiscordMember user)
        {
            await msg.Channel.SendMessageAsync(new DiscordEmbedBuilder
            {
                Title = "User's avatar",
                Description = $"**{user.Mention}'s avatar**",
                ImageUrl = user.AvatarUrl,
                Url = user.AvatarUrl,
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.MainEmbedColor
            });
        }
        
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