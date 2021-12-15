using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace ducker.Commands.MiscModule
{
    public partial class MiscCommands
    {
        /// <summary>
        /// Command to get random number in some range
        /// </summary>
        /// <param name="msg">The context that command belongs to</param>
        /// <param name="minValue">Minimal value of your range</param>
        /// <param name="maxValue">Maximum value of your range</param>
        [Command("random"), 
         Description("Send random value in your range from min to max value to current channel"),
         Aliases("rnd")]
        public async Task Random(CommandContext msg, [Description("min value")] int minValue, [Description("max value")] int maxValue)
        {
            var rnd = new Random();
            if (minValue > maxValue)
            {
                await msg.Channel.SendMessageAsync(new DiscordEmbedBuilder
                {
                    Title = "Missing argument",
                    Description = "**Usage:** `-random <min value> <max value>`",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.IncorrectEmbedColor
                });
                return;
            }
            await msg.Channel.SendMessageAsync(new DiscordEmbedBuilder
            {
                Title = "Random number",
                Description = $"Your random number is: **{rnd.Next(minValue, maxValue + 1)}**",
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
        [Command("random")]
        public async Task Random(CommandContext msg, [RemainingText] string text)
        {
            await msg.Channel.SendMessageAsync(new DiscordEmbedBuilder
            {
                Title = "Missing argument",
                Description = $"**Usage:** `-random <min value> <max value>`",
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