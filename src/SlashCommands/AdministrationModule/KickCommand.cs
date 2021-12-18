using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using ducker.Attributes;
using ducker.Logs;

namespace ducker.SlashCommands.AdministrationModule
{
    public partial class AdministrationSlashCommands
    {
        [SlashCommand("kick", "Kicks mentioned user from current server"), RequireAdmin]
        public async Task KickCommand(InteractionContext msg, [Option("user", "User for kick")] DiscordMember member, [Option("reason", "Reason for kick this member")] string reason = "No reason given")
        {
            await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
            try
            {
                await member.RemoveAsync(reason);
                await Log.LogToAudit(msg.Guild, $"{msg.Member.Mention} kicked {member.Mention}. Reason: {reason}");
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
}