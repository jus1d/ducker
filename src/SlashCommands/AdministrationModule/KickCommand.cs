using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using ducker.Logs;
using ducker.SlashCommands.Attributes;

namespace ducker.SlashCommands.AdministrationModule;

public partial class AdministrationSlashCommands
{
    [SlashCommand("kick", "Kicks mentioned user from current server")]
    [RequireAdmin]
    public async Task KickCommand(InteractionContext msg,
        [Option("user", "User for kick")] DiscordUser user,
        [Option("reason", "Reason for kick this member")]
        string reason = "noneReason")
    {
        await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
        var member = (DiscordMember) user;
        try
        {
            await member.RemoveAsync(reason);
            await Log.Audit(msg.Guild, $"{msg.Member.Mention} kicked {member.Mention}.", reason);
        }
        catch
        {
            await msg.Channel.SendMessageAsync(new DiscordEmbedBuilder
            {
                Description = "You can't kick this member",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.IncorrectEmbedColor
            });
            throw;
        }
    }
}