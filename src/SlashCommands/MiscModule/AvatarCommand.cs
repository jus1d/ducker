using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace ducker.SlashCommands.MiscModule;

public partial class MiscSlashCommands : ApplicationCommandModule
{
    [SlashCommand("avatar", "Send embed with users avatar to current channel")]
    public async Task Avatar(InteractionContext msg,
        [Option("User", "User, whose avatar you need")]
        DiscordUser user)
    {
        await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
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
}