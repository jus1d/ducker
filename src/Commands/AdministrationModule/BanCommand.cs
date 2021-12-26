using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using ducker.Commands.Attributes;
using ducker.Logs;

namespace ducker.Commands.AdministrationModule
{
    public partial class AdministrationCommands : BaseCommandModule
    {
        [Command("ban"),
         Description("Ban mentioned user in current server"),
         RequireAdmin]
        public async Task BanCommand(CommandContext msg, DiscordMember member, [RemainingText] string reason = "No reason given")
        {
            try
            {
                await member.Guild.BanMemberAsync(member, 0, reason);
                await msg.Message.CreateReactionAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
                await Log.Audit(msg.Guild, $"{msg.Member.Mention} banned {member.Mention}.", reason);
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

        [Command("ban")]
        public async Task BanCommand(CommandContext msg, [RemainingText] string text)
        {
            await msg.Channel.SendMessageAsync(Embed.IncorrectCommand(msg, "ban <member or reply to message>"));
        }
    }
}