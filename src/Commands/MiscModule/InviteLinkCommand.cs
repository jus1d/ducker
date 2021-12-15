using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace ducker.Commands.MiscModule
{
    public partial class MiscCommands
    {
        /// <summary>
        /// Command to get bot invite link
        /// </summary>
        /// <param name="msg">The context that command belongs to</param>
        [Command("invite-link"), 
         Description("Send invite link for this bot to current channel"),
         Aliases("invite")]
        public async Task InviteLink(CommandContext msg)
        {
            await msg.Channel.SendMessageAsync(new DiscordEmbedBuilder
            {
                Title = "Invite Link",
                Url = Bot.InviteLink,
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
        [Command("invite-link")]
        public async Task InviteLink(CommandContext msg, [RemainingText] string text)
        { 
            await msg.Channel.SendMessageAsync(new DiscordEmbedBuilder
            {
                Title = $"Missing argument",
                Description = $"**Usage:** `-invite-link`",
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