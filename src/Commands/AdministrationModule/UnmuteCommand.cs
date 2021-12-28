using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using ducker.Commands.Attributes;
using ducker.DiscordData;
using ducker.Database;
using ducker.Logs;

namespace ducker.Commands.AdministrationModule
{
    public partial class AdministrationCommands
    {
        [Command("unmute"), 
         Description("Unmute mentioned member"),
         RequireAdmin]
        public async Task UnmuteCommand(CommandContext msg, DiscordMember member, [RemainingText] string reason = "No reason given")
        {
            ulong muteRoleId = DB.GetId(msg.Guild.Id, "muteRoleId");
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
                await Log.Audit(msg.Guild, $"{msg.Member.Mention} unmuted {member.Mention}.", reason);
            }
        }
        
        [Command("unmute")]
        public async Task UnmuteCommand(CommandContext msg, [RemainingText] string text)
        {
            await msg.Channel.SendMessageAsync(Embed.IncorrectCommand(msg, "unmute <member> <reason>"));
        }
    }
}