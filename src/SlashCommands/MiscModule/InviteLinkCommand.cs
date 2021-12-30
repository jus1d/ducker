using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace ducker.SlashCommands.MiscModule;

public partial class MiscSlashCommands
{
    [SlashCommand("invite-link", "Send invite link for this bot to current channel")]
    public async Task InviteLink(InteractionContext msg)
    {
        await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
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
}