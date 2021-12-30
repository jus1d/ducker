using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using ducker.DiscordData;

namespace ducker.Commands.MiscModule;

public partial class MiscCommands : BaseCommandModule
{
    [Command("avatar")]
    [Description("Send user's avatar and it's link to current channel")]
    [Aliases("ava")]
    public async Task Avatar(CommandContext msg, DiscordMember member)
    {
        await msg.Message.CreateReactionAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
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

    [Command("avatar")]
    public async Task Avatar(CommandContext msg, [RemainingText] string text)
    {
        await msg.Message.CreateReactionAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
        if (msg.Message.ReferencedMessage == null)
        {
            await msg.Channel.SendMessageAsync(Embed.IncorrectCommand(msg,
                "avatar <member or reply to message>"));
            return;
        }

        await msg.Channel.SendMessageAsync(new DiscordEmbedBuilder
        {
            Title = "User's avatar",
            Description = $"**{msg.Message.ReferencedMessage.Author.Mention}'s avatar**",
            ImageUrl = msg.Message.ReferencedMessage.Author.AvatarUrl,
            Url = msg.Message.ReferencedMessage.Author.AvatarUrl,
            Footer = new DiscordEmbedBuilder.EmbedFooter
            {
                IconUrl = msg.User.AvatarUrl,
                Text = msg.User.Username
            },
            Color = Bot.MainEmbedColor
        });
    }
}