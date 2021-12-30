using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace ducker.Commands.MiscModule;

public partial class MiscCommands
{
    [Command("random")]
    [Description("Send random value in your range from min to max value to current channel")]
    [Aliases("rnd")]
    public async Task Random(CommandContext msg, [Description("min value")] int minValue,
        [Description("max value")] int maxValue)
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
        await msg.Message.CreateReactionAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
    }

    [Command("random")]
    public async Task Random(CommandContext msg, [RemainingText] string text)
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
    }
}