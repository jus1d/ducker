using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using ducker.Attributes;
using ducker.Logs;

namespace ducker.SlashCommands.AdministrationModule
{
    public partial class AdministrationSlashCommands
    {
        [SlashCommand("ban", "Ban mentioned user in current server"), RequireAdmin]
        public async Task BanCommand(InteractionContext msg, [Option("user", "User for ban")] DiscordMember member, [Option("reason", "Reason for ban this user")] string reason = "No reason given")
        {
            await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
            
            try
            {
                await msg.Guild.BanMemberAsync(member, 0, reason);
                await Log.LogToAudit(msg.Guild, $"{msg.Member.Mention} banned {member.Mention}. Reason: {reason}");
            }
            catch
            {
                await msg.Channel.SendMessageAsync(new DiscordEmbedBuilder
                {
                    Description = "You can't ban this user",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.WarningColor
                });
            }
        }
    }
}