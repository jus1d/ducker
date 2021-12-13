using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace ducker.Commands.AdministrationModule
{
    public partial class AdministrationCommands : BaseCommandModule
    {
        [Command("ban"),
         Description("Ban mentioned user in current server"),
         RequirePermissions(Permissions.BanMembers)]
        public async Task BanCommand(CommandContext msg, DiscordMember user, [RemainingText] string reason = "Reason does not given")
        {
            try
            {
                await user.Guild.BanMemberAsync(user, 0, reason);
                await msg.Message.CreateReactionAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
            }
            catch (Exception e)
            {
                var incorrectBanCommandEmbed = new DiscordEmbedBuilder
                {
                    Description = $":x: You can't ban this user",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.IncorrectEmbedColor
                };
                await msg.Channel.SendMessageAsync(incorrectBanCommandEmbed);
            }
        }

        [Command("ban"),
         Description("Ban mentioned user in current server"),
         RequirePermissions(Permissions.BanMembers)]
        public async Task BanCommand(CommandContext msg, [RemainingText] string text)
        {
            var incorrectBanCommandEmbed = new DiscordEmbedBuilder
            {
                Title = $"Missing argument",
                Description = $"**Usage:** `-ban <member>`",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.IncorrectEmbedColor
            };
            await msg.Channel.SendMessageAsync(incorrectBanCommandEmbed);
        }
    }
}