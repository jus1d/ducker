using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using ducker.Attributes;
using ducker.Database;
using ducker.Logs;

namespace ducker.Commands.AdministrationModule
{
    public partial class AdministrationModule
    {
        [Command("unmute"), 
         Description("Unmute mentioned member"),
         RequireAdmin]
        public async Task Unmute(CommandContext msg, DiscordMember member, [RemainingText] string reason)
        {
            ulong muteRoleId = DB.GetMuteRoleId(msg.Guild.Id);
            if (muteRoleId == 0)
            {
                await msg.Channel.SendMessageAsync(new DiscordEmbedBuilder
                {
                    Description = "Mute role is not configured for this server\nUse `mute` to configure it",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.WarningColor
                });
            }
            else
            {
                await member.RevokeRoleAsync(msg.Guild.GetRole(muteRoleId));
                await msg.Message.CreateReactionAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
                await Log.LogToAudit(msg.Guild, $"{msg.Member.Mention} unmuted {member.Mention}. Reason: {reason}");
            }
        }
        
        [Command("unmute")]
        public async Task Unmute(CommandContext msg, [RemainingText] string text)
        {
            await msg.Channel.SendMessageAsync(new DiscordEmbedBuilder
            {
                Title = "Missing argument",
                Description = "**Usage:** `-unmute <member>`",
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