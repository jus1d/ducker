using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace ducker.Events;

public partial class EventHandler
{
    public static async Task OnMessageCreated(DiscordClient client, MessageCreateEventArgs msg)
    {
        if (msg.Author.IsBot)
            return;

        var member = (DiscordMember) msg.Author;

        if (msg.Message.MentionEveryone)
        {
            if (member.Guild.Permissions == Permissions.Administrator) // ignore admins
                return;

            await msg.Message.DeleteAsync();
            await msg.Message.Channel.SendMessageAsync(new DiscordEmbedBuilder
            {
                Title = "Don't tag everyone!",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.Author.AvatarUrl,
                    Text = msg.Author.Username
                },
                Color = Bot.IncorrectEmbedColor
            });
        }
    }
}